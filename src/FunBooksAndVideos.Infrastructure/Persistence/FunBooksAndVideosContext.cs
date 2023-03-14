using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.SeedWork;
using System.Reflection;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.Shipping;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public class FunBooksAndVideosContext : DbContext, IFunBooksAndVideosContext
{
    private readonly IMediator _mediator;

    public FunBooksAndVideosContext(
        DbContextOptions<FunBooksAndVideosContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductType> ProductTypes => Set<ProductType>();

    public DbSet<ShippingSlip> ShippingSlips => Set<ShippingSlip>();

    public DbSet<T> GetDbSet<T>() where T : class
    {
        return Set<T>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        var events = entries.SelectMany(e => e.Entity.Events).ToList();

        foreach (var entry in entries)
        {
            entry.Entity.ClearEvents();
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var @event in events)
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .Ignore<DomainEvent>();
    }
}
