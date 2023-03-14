using MediatR;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomerQuery : IRequest<CustomerDto>
{
    /// <summary>
    /// Customer ID.
    /// </summary>
    /// <value></value>
    public int CustomerId { get; set; }
}
