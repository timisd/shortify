using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
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
        services.AddSingleton<JsonHelper>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ApiSettings settings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Shortify",
                ValidAudience = "Shortify",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.EncryptionKey))
            };
        });

        return services;
    }
}