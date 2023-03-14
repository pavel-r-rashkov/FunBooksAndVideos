using AutoMapper;
using FunBooksAndVideos.Domain.Customers;

namespace FunBooksAndVideos.Application.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
    }
}
