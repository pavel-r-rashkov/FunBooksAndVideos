using FluentValidation;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Application.Profiles;

public class GetMembershipsValidator : AbstractValidator<GetMembershipsQuery>
{
    private static readonly IEnumerable<string> _properties = typeof(MembershipDto)
        .GetProperties()
        .Select(p => p.Name);

    public GetMembershipsValidator()
    {
        RuleFor(m => m.Pagination)
            .SetValidator(new PaginationValidator());

        RuleForEach(m => m.Filters)
            .SetValidator(new FilterValidator(_properties));

        RuleForEach(m => m.SortOrders)
            .SetValidator(new SortOrderValidator(_properties));
    }
}
