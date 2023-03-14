using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

internal sealed class CollectionFilter : IParameterFilter
{
    private const string StringSchemaType = "string";

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ApiParameterDescription?.ModelMetadata?.ContainerType == typeof(Pagination))
        {
            parameter.In = ParameterLocation.Query;
            parameter.Required = false;
            parameter.Name = parameter.Name
                .Split('.')
                .Last()
                .ToLowerInvariant();

            return;
        }

        var elementType = context.ApiParameterDescription?.ModelMetadata?.ElementType;

        if (elementType == typeof(Filter))
        {
            parameter.Name = FilterBinder.QueryParamName;
            parameter.In = ParameterLocation.Query;
            parameter.Required = false;
            parameter.Schema = new OpenApiSchema
            {
                Type = StringSchemaType,
            };
            parameter.Examples = new Dictionary<string, OpenApiExample>
            {
                {
                    "Example0",
                    new OpenApiExample
                    {
                        Summary = "empty",
                        Value = new OpenApiString(string.Empty),
                    }
                },
                {
                    "Example1",
                    new OpenApiExample
                    {
                        Summary = "propertyName greater than 5",
                        Value = new OpenApiString("propertyName gt 5"),
                    }
                },
                {
                    "Example2",
                    new OpenApiExample
                    {
                        Summary = "propertyName equals 5",
                        Value = new OpenApiString("propertyName eq 5"),
                    }
                },
                {
                    "Example3",
                    new OpenApiExample
                    {
                        Summary = "multiple properties",
                        Value = new OpenApiString("propertyName1 eq 1, propertyName2 gt 5, propertyName3 lt 2"),
                    }
                },
                {
                    "Example4",
                    new OpenApiExample
                    {
                        Summary = "search by string property",
                        Value = new OpenApiString("name eq 'John Doe'"),
                    }
                },
                {
                    "Example5",
                    new OpenApiExample
                    {
                        Summary = "escape example",
                        Value = new OpenApiString("name eq 'John\\'s'"),
                    }
                },
            };

            return;
        }

        if (elementType == typeof(SortOrder))
        {
            parameter.Name = SortOrderBinder.QueryParamName;
            parameter.In = ParameterLocation.Query;
            parameter.Required = false;
            parameter.Schema = new OpenApiSchema
            {
                Type = StringSchemaType,
            };
            parameter.Examples = new Dictionary<string, OpenApiExample>
            {
                {
                    "Example0",
                    new OpenApiExample
                    {
                        Summary = "empty",
                        Value = new OpenApiString(string.Empty),
                    }
                },
                {
                    "Example1",
                    new OpenApiExample
                    {
                        Summary = "sort in ascending order",
                        Value = new OpenApiString("propertyName asc"),
                    }
                },
                {
                    "Example2",
                    new OpenApiExample
                    {
                        Summary = "sort in descending order",
                        Value = new OpenApiString("propertyName desc"),
                    }
                },
                {
                    "Example3",
                    new OpenApiExample
                    {
                        Summary = "if not specified sort is ascending",
                        Value = new OpenApiString("propertyName"),
                    }
                },
                {
                    "Example4",
                    new OpenApiExample
                    {
                        Summary = "sort by multiple properties",
                        Value = new OpenApiString("propertyName1 asc, propertyName2 desc"),
                    }
                },
            };

            return;
        }
    }
}
