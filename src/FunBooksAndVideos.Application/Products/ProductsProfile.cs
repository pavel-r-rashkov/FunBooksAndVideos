using AutoMapper;
using FunBooksAndVideos.Domain.Products;

namespace FunBooksAndVideos.Application.Products;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(p => p.ProductTypeName, opt => opt.MapFrom(p => p.ProductType.Name));
    }
}
