using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Shortify.Persistence.EfCore;

public class DbConnectionFactory(ILogger<DbConnectionFactory> logger, IOptions<DatabaseSettings> settings)
{
    public NpgsqlConnection GetConnection()
    {
        var conString = GenerateConnectionString() ?? settings.Value.ConnectionString;

        logger.LogError("Connection string: {0}", conString);

        var connection = new NpgsqlConnection(GenerateConnectionString() ?? settings.Value.ConnectionString);
        connection.Open();
        return connection;
    }

    private string? GenerateConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("DatabaseSettings__Host");
        var port = Environment.GetEnvironmentVariable("DatabaseSettings__Port");
        var database = Environment.GetEnvironmentVariable("DatabaseSettings__Database");
        var username = Environment.GetEnvironmentVariable("DatabaseSettings__Username");
        var password = Environment.GetEnvironmentVariable("DatabaseSettings__Password");

        if (host is null || port is null || database is null || username is null || password is null) return null;

        return new DatabaseSettings
        {
            Host = host,
            Port = int.Parse(port),
            Database = database,
            Username = username,
            Password = password
        }.ConnectionString;
    }
}