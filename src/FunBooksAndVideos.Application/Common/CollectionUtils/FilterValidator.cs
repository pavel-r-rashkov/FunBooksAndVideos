using FluentValidation;

namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class FilterValidator : AbstractValidator<Filter>
{
    public FilterValidator(IEnumerable<string> allowedProperties)
    {
        RuleFor(f => f.PropertyName)
            .Must(p => allowedProperties
                .Any(ap => ap.Equals(p, StringComparison.OrdinalIgnoreCase)))
            .WithMessage($"Allowed properties for filtering: {string.Join(", ", allowedProperties)}");
    }
}
