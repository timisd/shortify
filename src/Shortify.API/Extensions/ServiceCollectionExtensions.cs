using Shortify.Common.Misc;
using Shortify.Common.Models;
using Shortify.Persistence;
using Shortify.Persistence.EfCore;
using Shortify.Persistence.EfCore.Repositories;

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

    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<PasswordHelper>();
        services.AddSingleton<JwtTokenHelper>();
        services.AddSingleton<UrlGenerator>();
        return services;
    }
}