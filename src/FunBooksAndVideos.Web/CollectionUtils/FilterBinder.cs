using Microsoft.AspNetCore.Mvc.ModelBinding;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

public class FilterBinder : IModelBinder
{
    public const string QueryParamName = "filter";
    private const string NullToken = "null";
    private const string StringLiteralToken = "'";
    private const char StringLiteralCharToken = '\'';
    private const string StringEscapeToken = @"\'";
    private const char StringEscapeCharToken = '\\';

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var filterParam = bindingContext.HttpContext.Request.Query[QueryParamName];

        if (!filterParam.Any())
        {
            return Task.CompletedTask;
        }

        var filters = filterParam
            .Where(p => !string.IsNullOrEmpty(p))
            .SelectMany(p => SplitFilter(p!))
            .Select(filter =>
            {
                if (filter.Count != 3)
                {
                    throw new CollectionBindingException("Invalid filter");
                }

                var type = filter[1] switch
                {
                    "eq" => FilterOperator.Equal,
                    "ne" => FilterOperator.NotEqual,
                    "lt" => FilterOperator.LessThan,
                    "le" => FilterOperator.LessThanOrEqual,
                    "gt" => FilterOperator.GreaterThan,
                    "ge" => FilterOperator.GreaterThanOrEqual,
                    "co" => FilterOperator.Contains,
                    _ => throw new CollectionBindingException("Filter type must be one of \"eq\", \"ne\", \"lt\", \"le\", \"gt\", \"ge\", \"co\""),
                };

                return new Filter
                {
                    PropertyName = filter[0],
                    Operator = type,
                    Value = GetFilterValue(filter[2]),
                };
            });

        bindingContext.Result = ModelBindingResult.Success(filters);

        return Task.CompletedTask;
    }

    private static string? GetFilterValue(string token)
    {
        if (string.Equals(token, NullToken, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (token.Length < 2)
        {
            return token;
        }

        if (!token.StartsWith(StringLiteralToken, StringComparison.OrdinalIgnoreCase)
            || !token.EndsWith(StringLiteralToken, StringComparison.OrdinalIgnoreCase))
        {
            return token;
        }

        return token[1..^1].Replace(StringEscapeToken, StringLiteralToken);
    }

    private static IEnumerable<IList<string>> SplitFilter(string filter)
    {
        var filters = new List<IList<string>>();
        var tokens = new List<string>();
        var inLiteral = false;
        var last = -1;

        for (var i = 0; i < filter.Length; i++)
        {
            var character = filter[i];

            if (inLiteral)
            {
                if (!character.Equals(StringLiteralCharToken))
                {
                    continue;
                }

                if (i - 1 >= 0 && filter[i - 1].Equals(StringEscapeCharToken))
                {
                    continue;
                }

                tokens.Add(filter[last..(i + 1)].Replace(StringEscapeToken, "'"));
                last = i;
                inLiteral = false;
            }
            else
            {
                if (char.IsWhiteSpace(character))
                {
                    tokens.Add(filter[(last + 1)..i]);
                    last = i;
                }
                else if (character.Equals(StringLiteralCharToken))
                {
                    inLiteral = true;
                    tokens.Add(filter[(last + 1)..i]);
                    last = i;
                }
                else if (character.Equals(','))
                {
                    tokens.Add(filter[(last + 1)..i]);
                    last = i;
                    filters.Add(tokens.Where(t => !string.IsNullOrEmpty(t)).ToList());
                    tokens = new List<string>();
                }
            }
        }

        tokens.Add(filter[(last + 1)..filter.Length]);
        filters.Add(tokens.Where(t => !string.IsNullOrEmpty(t)).ToList());

        return filters;
    }
}
