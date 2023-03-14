using FunBooksAndVideos.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    private const string MEMBERSHIP_PK = "Id";

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.OwnsMany(
            c => c.Memberships,
            m =>
            {
                m.ToTable(nameof(Customer.Memberships)).WithOwner().HasForeignKey(i => i.CustomerId);
                m.Property<int>(MEMBERSHIP_PK);
                m.HasKey(MEMBERSHIP_PK);
            });
    }
}
