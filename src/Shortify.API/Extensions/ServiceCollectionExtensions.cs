using Shortify.Common.Models;
using Shortify.Persistence;
using Shortify.Persistence.EfCore;
using Shortify.Persistence.EfCore.Repositories;
using Shortify.Persistence.Models;

namespace Shortify.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeneralSettings>(configuration.GetSection("GeneralSettings"));
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<DbConnectionFactory>();
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUrlRepository, EfCoreUrlRepository>();
        services.AddScoped<IUserRepository, EfCoreUserRepository>();

        return services;
    }
}