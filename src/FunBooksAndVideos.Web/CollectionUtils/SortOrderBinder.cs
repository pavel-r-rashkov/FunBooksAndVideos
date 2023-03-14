using Microsoft.AspNetCore.Mvc.ModelBinding;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

public class SortOrderBinder : IModelBinder
{
    public const string QueryParamName = "orderBy";

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var orderParam = bindingContext.HttpContext.Request.Query[QueryParamName];

        if (!orderParam.Any())
        {
            return Task.CompletedTask;
        }

        var sortOrderTokens = orderParam
            .Where(p => !string.IsNullOrEmpty(p))
            .SelectMany(p => p!.Split(',', StringSplitOptions.RemoveEmptyEntries));

        var sortOrders = sortOrderTokens.Select(s =>
        {
            var sortTokens = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var direction = SortDirection.Ascending;

            if (sortTokens.Length > 2)
            {
                throw new CollectionBindingException("Invalid sort condition");
            }

            if (sortTokens.Length == 2)
            {
                direction = sortTokens[1] switch
                {
                    "asc" => SortDirection.Ascending,
                    "desc" => SortDirection.Descending,
                    _ => throw new CollectionBindingException("Sort direction must be \"asc\" or \"desc\""),
                };
            }

            return new SortOrder
            {
                PropertyName = sortTokens[0],
                Direction = direction,
            };
        });

        bindingContext.Result = ModelBindingResult.Success(sortOrders);

        return Task.CompletedTask;
    }
}
