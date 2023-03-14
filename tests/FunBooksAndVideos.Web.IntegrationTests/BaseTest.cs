using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FunBooksAndVideos.Infrastructure.Persistence;
using Xunit;
using Microsoft.Extensions.Options;
using FunBooksAndVideos.Application.Common;
using DbUp;
using DbUp.Helpers;
using Microsoft.EntityFrameworkCore;
using DbUp.Engine;

namespace FunBooksAndVideos.Web.IntegrationTests;

[Collection(Constants.TestCollection)]
public class BaseTest : IDisposable, IAsyncLifetime
{
    private readonly UpgradeEngine _upgradeEngine;
    private bool _disposedValue;

    public BaseTest(
        TestWebApplicationFactory appFactory)
    {
        AppFactory = appFactory;
        Client = appFactory.CreateClient();
        var scope = appFactory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<FunBooksAndVideosContext>();
        var connectionString = scope.ServiceProvider.GetRequiredService<IOptions<AppConfig>>()
            .Value.ConnectionStrings!.FunBooksAndVideosConnectionString;

        _upgradeEngine = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Database.Program).Assembly, (s) => s.Contains("seed"))
            .JournalTo(new NullJournal())
            .Build();
    }

    protected TestWebApplicationFactory AppFactory { get; }

    protected FunBooksAndVideosContext Context { get; }

    protected HttpClient Client { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task DisposeAsync()
    {
        // Clean tables after each test.
        await Context.ShippingSlips.ExecuteDeleteAsync();
        await Context.Orders.ExecuteDeleteAsync();
        await Context.ProductTypes.ExecuteDeleteAsync();
        await Context.Products.ExecuteDeleteAsync();
        await Context.Customers.ExecuteDeleteAsync();
    }

    public Task InitializeAsync()
    {
        // Seed data before each test.
        _upgradeEngine.PerformUpgrade();

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Client.Dispose();
            }

            _disposedValue = true;
        }
    }
}
