using FluentValidation;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomerValidator : AbstractValidator<GetCustomerQuery>
{
    public GetCustomerValidator()
    {
        RuleFor(m => m.CustomerId).GreaterThan(0);
    }
}
