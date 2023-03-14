using FluentValidation;
using static FunBooksAndVideos.Application.Orders.CreateOrderCommand;

namespace FunBooksAndVideos.Application.Orders;

public class CreateOrderLineItemValidator : AbstractValidator<CreateOrderLineItemDto>
{
    public CreateOrderLineItemValidator()
    {
        RuleFor(m => m.ProductId).GreaterThan(0);

        RuleFor(m => m.Quantity).GreaterThan(0);
    }
}
