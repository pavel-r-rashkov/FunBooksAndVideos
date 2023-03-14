using FluentValidation;

namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class PaginationValidator : AbstractValidator<Pagination>
{
    private const int DefaultMaxTake = 100;
    private readonly int _maxSize;

    public PaginationValidator()
    {
        _maxSize = DefaultMaxTake;
        ConfigureValidation();
    }

    public PaginationValidator(int maxSize)
    {
        _maxSize = maxSize;
        ConfigureValidation();
    }

    private void ConfigureValidation()
    {
        RuleFor(p => p.Take)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(_maxSize);

        RuleFor(p => p.Skip)
            .GreaterThanOrEqualTo(0);
    }
}
