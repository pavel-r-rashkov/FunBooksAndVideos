using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Orders;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerOrdersValidator : AbstractValidator<GetCustomerOrdersQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(OrderDto)
        .GetProperties()
        .Where(p => !p.Name.Equals(nameof(OrderDto.OrderLineItems), StringComparison.OrdinalIgnoreCase))
        .Select(p => p.Name);

    public GetCustomerOrdersValidator()
    {
        RuleFor(m => m.Pagination).SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters).SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders).SetValidator(new SortOrderValidator(_properties));
    }
}
