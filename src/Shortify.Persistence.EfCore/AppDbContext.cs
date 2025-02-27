using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shortify.Common.Models;

namespace Shortify.Persistence.EfCore;

public class AppDbContext(ILogger<AppDbContext> logger, DbConnectionFactory dbConnectionFactory) : DbContext
{
    public DbSet<Url> Urls { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ShortLink).IsUnique();
            entity.HasIndex(e => e.UserMail);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connection = dbConnectionFactory.GetConnection();
            optionsBuilder.UseNpgsql(connection);
            logger.LogDebug("Database connection to {Connection} configured.", connection.ToString());
        }

        base.OnConfiguring(optionsBuilder);
    }
}