using FunBooksAndVideos.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public class ShippingSlipConfiguration : IEntityTypeConfiguration<ShippingSlip>
{
    public void Configure(EntityTypeBuilder<ShippingSlip> builder)
    {
        builder.OwnsMany(o => o.DeliveryItems, m =>
        {
            m.WithOwner().HasForeignKey(p => p.ShippingSlipId);
            m.HasKey(i => i.Id);
            m.ToTable(nameof(ShippingSlip.DeliveryItems));
        });
    }
}
