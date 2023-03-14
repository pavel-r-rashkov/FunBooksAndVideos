using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Products;

public class ProductType : Entity
{
    public ProductType(int id, string name, bool isPhysical, bool requiresActivation)
    {
        Id = id;
        Name = name;
        IsPhysical = isPhysical;
        RequiresActivation = requiresActivation;
    }

    public string Name { get; private set; }

    public bool IsPhysical { get; private set; }

    public bool RequiresActivation { get; private set; }
}
