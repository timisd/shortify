using Microsoft.Extensions.Options;
using Npgsql;
using Shortify.Persistence.Models;

namespace Shortify.Persistence.EfCore;

public class DbConnectionFactory(IOptions<DatabaseSettings> settings)
{
    public NpgsqlConnection GetConnection()
    {
        var connection = new NpgsqlConnection(settings.Value.ConnectionString);
        connection.Open();
        return connection;
    }
}