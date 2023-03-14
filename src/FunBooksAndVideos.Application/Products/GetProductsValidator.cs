using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Application.Products;

public class GetProductsValidator : AbstractValidator<GetProductsQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(ProductDto)
        .GetProperties()
        .Select(p => p.Name);

    public GetProductsValidator()
    {
        RuleFor(m => m.Pagination).SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters).SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders).SetValidator(new SortOrderValidator(_properties));
    }
}
