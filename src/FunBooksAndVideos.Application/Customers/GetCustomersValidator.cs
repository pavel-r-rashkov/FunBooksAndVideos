using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomersValidator : AbstractValidator<GetCustomersQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(CustomerDto)
        .GetProperties()
        .Select(p => p.Name);

    public GetCustomersValidator()
    {
        RuleFor(m => m.Pagination).SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters).SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders).SetValidator(new SortOrderValidator(_properties));
    }
}
