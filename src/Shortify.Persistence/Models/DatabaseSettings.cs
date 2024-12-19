namespace Shortify.Persistence.Models;

public class DatabaseSettings
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public required string Database { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
}