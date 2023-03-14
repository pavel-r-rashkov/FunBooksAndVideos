using FluentValidation;

namespace FunBooksAndVideos.Application.Orders;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly CreateOrderLineItemValidator _orderLineItemValidator = new();

    public CreateOrderValidator()
    {
        RuleFor(m => m.OrderLineItems)
            .NotNull()
            .NotEmpty()
            .ForEach(i => i.SetValidator(_orderLineItemValidator));
    }
}
