using FluentValidation;

namespace FunBooksAndVideos.Application.Products;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(512);

        RuleFor(m => m.Price)
            .GreaterThan(0);

        RuleFor(m => m.ProductTypeId)
            .GreaterThan(0);
    }
}
