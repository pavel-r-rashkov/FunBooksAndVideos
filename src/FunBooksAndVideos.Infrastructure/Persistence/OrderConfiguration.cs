using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .OwnsMany(o => o.OrderLineItems, m =>
            {
                m.ToTable(nameof(Order.OrderLineItems))
                    .WithOwner()
                    .HasForeignKey(p => p.OrderId);
                m.HasKey(i => i.Id);
                m.Property(i => i.OrderId).HasColumnName(nameof(OrderLineItem.OrderId));
                m.OwnsOne(i => i.ProductQuantity, pq =>
                {
                    pq.Property(p => p.ProductId)
                        .IsRequired()
                        .HasColumnName(nameof(ProductQuantity.ProductId));

                    pq.Property(p => p.Quantity)
                        .IsRequired()
                        .HasColumnName(nameof(ProductQuantity.Quantity));
                });
            })
            .Property(m => m.OrderNumber)
            .HasDefaultValueSql("NEXT VALUE FOR OrderNumbers");
    }
}
