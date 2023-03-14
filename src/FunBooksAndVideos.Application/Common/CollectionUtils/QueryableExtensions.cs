using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public static class QueryableExtensions
{
    private static readonly MethodInfo _changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) })!;
    private static readonly MethodInfo _containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) })!;

    public static async Task<PagedResult<T>> ToPagedResult<T>(this IQueryable<T> queryable, Pagination? pagination, CancellationToken cancellationToken = default)
    {
        if (pagination == null)
        {
            throw new ArgumentNullException(nameof(pagination));
        }

        var count = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Skip(pagination.Skip)
            .Take(pagination.Take)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, count);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<SortOrder>? sortOrders)
    {
        if (sortOrders == null)
        {
            return queryable;
        }

        IOrderedQueryable<T>? ordered = default;

        foreach (var sortOrder in sortOrders)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, sortOrder.PropertyName);
            var propertyAsObject = Expression.Convert(property, typeof(object));

            var orderLambda = Expression.Lambda<Func<T, object>>(propertyAsObject, parameter);

            if (ordered == default)
            {
                ordered = sortOrder.Direction == SortDirection.Ascending
                    ? queryable.OrderBy(orderLambda)
                    : queryable.OrderByDescending(orderLambda);

                continue;
            }

            ordered = sortOrder.Direction == SortDirection.Ascending
                ? ordered.ThenBy(orderLambda)
                : ordered.ThenByDescending(orderLambda);
        }

        return ordered ?? queryable;
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IEnumerable<Filter>? filters)
    {
        if (filters == null || !filters.Any())
        {
            return queryable;
        }

        Expression? combinedFilterExpression = default;
        var parameter = Expression.Parameter(typeof(T));

        foreach (var filter in filters)
        {
            var property = Expression.Property(parameter, filter.PropertyName);

            if (filter.Operator == FilterOperator.Contains)
            {
                var containsExpression = Expression.Call(property, _containsMethod, Expression.Constant(filter.Value));
                combinedFilterExpression = combinedFilterExpression == default
                    ? containsExpression
                    : Expression.AndAlso(combinedFilterExpression, containsExpression);

                continue;
            }

            var expressionType = filter.Operator switch
            {
                FilterOperator.Equal => ExpressionType.Equal,
                FilterOperator.NotEqual => ExpressionType.NotEqual,
                FilterOperator.LessThan => ExpressionType.LessThan,
                FilterOperator.LessThanOrEqual => ExpressionType.LessThanOrEqual,
                FilterOperator.GreaterThan => ExpressionType.GreaterThan,
                FilterOperator.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
                FilterOperator.Contains => throw new InvalidOperationException("Invalid filter operation"),
                _ => throw new ArgumentException("Unknown filter operator"),
            };

            var propertyType = ((PropertyInfo)property.Member).PropertyType;

            var filterValue = Expression.Call(
                _changeTypeMethod,
                Expression.Constant(filter.Value),
                Expression.Constant(Nullable.GetUnderlyingType(propertyType) ?? propertyType));

            var binaryExpression = Expression.MakeBinary(
                expressionType,
                property,
                Expression.Convert(filterValue, propertyType));

            combinedFilterExpression = combinedFilterExpression == default
                ? binaryExpression
                : Expression.AndAlso(combinedFilterExpression, binaryExpression);
        }

        var filterLambda = Expression.Lambda<Func<T, bool>>(combinedFilterExpression!, parameter);

        return queryable.Where(filterLambda);
    }
}
