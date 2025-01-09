using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Common.Models;

namespace Shortify.Persistence.EfCore.Repositories;

public class EfCoreUserRepository(
    ILogger<EfCoreUserRepository> logger,
    AppDbContext dbContext,
    PasswordHelper passwordHelper) : IUserRepository
{
    public async Task<User?> AddUserAsync(User user, CancellationToken ct = default)
    {
        try
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(ct);
            return user;
        }
        catch (DbUpdateException ex)
        {
            logger.LogDebug(ex, "Error adding url");
            return null;
        }
    }

    public async Task<User?> DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, ct);
            if (user == null) return user;

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(ct);
            return user;
        }
        catch (DbUpdateException ex)
        {
            logger.LogDebug(ex, "Error deleting url");
            return null;
        }
    }

    public Task<User?> UpdateUserAsync(User user, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogDebug(ex, "Could not get Url");
            return null;
        }
    }

    public async Task<PagedResult<User>> GetUsersAsync(Filter? filter, CancellationToken ct = default)
    {
        if (filter == null)
            return new PagedResult<User>
            {
                Items = await dbContext.Users.ToListAsync(ct),
                TotalItems = await dbContext.Users.CountAsync(ct),
                CurrentPage = -1,
                TotalPages = -1
            };

        var query = QueryHelper.ApplyFilter(dbContext.Users.AsQueryable(), filter);

        var totalItems = await query.CountAsync(ct);

        if (filter.StartPage == -1)
        {
            var allItems = await query.ToListAsync(ct);
            return new PagedResult<User>
            {
                Items = allItems,
                TotalItems = allItems.Count,
                CurrentPage = -1,
                TotalPages = 1
            };
        }

        var totalPages = (int)Math.Ceiling(totalItems / (double)filter.ItemsPerPage);

        var items = await query
            .Skip((filter.StartPage - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync(ct);

        return new PagedResult<User>
        {
            Items = items,
            TotalItems = totalItems,
            CurrentPage = filter.StartPage,
            TotalPages = totalPages
        };
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email, ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Could not get Url");
            return null;
        }
    }

    public async Task<User?> GetUserByLoginAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Users.FirstOrDefaultAsync(
                user => user.Email == email && user.PasswordHash == passwordHelper.ComputeSha256Hash(password), ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Could not get Url");
            return null;
        }
    }
}