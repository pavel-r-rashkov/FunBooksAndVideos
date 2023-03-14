using FluentValidation;

namespace FunBooksAndVideos.Application.Products;

public class GetProductValidator : AbstractValidator<GetProductQuery>
{
    public GetProductValidator()
    {
        RuleFor(m => m.ProductId).GreaterThan(0);
    }
}
