using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Products;

public class Product : Entity
{
    private Product()
    {
        // ctor for EF
    }

    public Product(int id, string name, decimal price, int productTypeId, ProductType productType)
    {
        Id = id;
        Name = name;
        Price = price;
        ProductTypeId = productTypeId;
        ProductType = productType;
    }

    public string Name { get; private set; } = default!;

    public decimal Price { get; private set; }

    public int ProductTypeId { get; private set; }

    public virtual ProductType ProductType { get; private set; } = default!;
}
