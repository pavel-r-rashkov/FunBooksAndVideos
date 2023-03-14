using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Infrastructure.Persistence;
using FunBooksAndVideos.Web.CollectionUtils;
using FunBooksAndVideos.Application.Customers;
using System.Reflection;

namespace FunBooksAndVideos.Web;

public static class Extensions
{
    public static WebApplicationBuilder AddAppConfig(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppConfig>(builder.Configuration);

        return builder;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        // TODO add log sink

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddConsole();
        }

        return builder;
    }

    public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(config =>
        {
            config.Filters.Add<ApiExceptionFilterAttribute>();

            config.ModelBinderProviders.Insert(0, new FilterBinderProvider());
            config.ModelBinderProviders.Insert(0, new SortOrderBinderProvider());
            config.ModelBinderProviders.Insert(0, new PaginationBinderProvider());
        });

        return builder;
    }

    public static WebApplicationBuilder AddAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(GetCustomersHandler).Assembly);

        return builder;
    }

    public static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(GetCustomersHandler).Assembly);

        ValidatorOptions.Global.DisplayNameResolver = (type, member, f) =>
        {
            return member?.Name is null
                ? string.Empty
                : member.Name;
        };

        return builder;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);
                options.ParameterFilter<CollectionFilter>();
            });

        return builder;
    }

    public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCustomersHandler).Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return builder;
    }

    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IIdentityProvider, FakeIdentityProvider>();

        return builder;
    }

    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDbContext<FunBooksAndVideosContext>(options =>
            {
                var key = nameof(AppConfig.ConnectionStringsSection.FunBooksAndVideosConnectionString);
                options.UseSqlServer(builder.Configuration.GetConnectionString(key));
            })
            .AddScoped<IFunBooksAndVideosContext, FunBooksAndVideosContext>();

        return builder;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }

        return app;
    }

    public static WebApplication MigrateAndRun(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var config = scope.ServiceProvider.GetRequiredService<IOptions<AppConfig>>().Value;
        var message = LoggerMessage.Define(LogLevel.Error, new EventId(), "Error staring application");

        try
        {
            if (!config.SkipDatabaseMigrations)
            {
                Database.Program.PerformUpgrade(config.ConnectionStrings!.FunBooksAndVideosConnectionString!, logger);
            }

            app.Run();
        }
        catch (Exception ex)
        {
            message(logger, ex);
            throw;
        }

        return app;
    }
}
