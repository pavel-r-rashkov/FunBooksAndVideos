using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Shipments;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerShipmentsValidator : AbstractValidator<GetCustomerShipmentsQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(ShipmentDto)
        .GetProperties()
        .Where(p => !p.Name.Equals(nameof(ShipmentDto.DeliveryItems), StringComparison.OrdinalIgnoreCase))
        .Select(p => p.Name);

    public GetCustomerShipmentsValidator()
    {
        RuleFor(m => m.Pagination).SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters).SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders).SetValidator(new SortOrderValidator(_properties));
    }
}
