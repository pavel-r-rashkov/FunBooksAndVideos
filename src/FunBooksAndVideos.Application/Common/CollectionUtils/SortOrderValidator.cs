using FluentValidation;

namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class SortOrderValidator : AbstractValidator<SortOrder>
{
    public SortOrderValidator(IEnumerable<string> allowedProperties)
    {
        RuleFor(f => f.PropertyName)
            .Must(p => allowedProperties
                .Any(ap => ap.Equals(p, StringComparison.OrdinalIgnoreCase)))
                .WithMessage($"Allowed properties for sorting: {string.Join(", ", allowedProperties)}");
    }
}
