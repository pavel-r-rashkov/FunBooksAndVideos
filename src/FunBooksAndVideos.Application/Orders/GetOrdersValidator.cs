using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Application.Orders;

public class GetOrdersValidator : AbstractValidator<GetOrdersQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(OrderDto)
        .GetProperties()
        .Where(p => !p.Name.Equals(nameof(OrderDto.OrderLineItems), StringComparison.OrdinalIgnoreCase))
        .Select(p => p.Name);

    public GetOrdersValidator()
    {
        RuleFor(m => m.Pagination).SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters).SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders).SetValidator(new SortOrderValidator(_properties));
    }
}
