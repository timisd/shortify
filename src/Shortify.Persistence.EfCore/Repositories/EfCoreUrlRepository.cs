using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;

namespace Shortify.Persistence.EfCore.Repositories;

public class EfCoreUrlRepository(ILogger<EfCoreUrlRepository> logger, AppDbContext dbContext) : IUrlRepository
{
    public async Task<Url?> AddUrlAsync(Url url, CancellationToken ct = default)
    {
        try
        {
            dbContext.Urls.Add(url);
            await dbContext.SaveChangesAsync(ct);
            return url;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error adding url");
            return null;
        }
    }

    public async Task<Url?> UpdateUrlAsync(Url url, CancellationToken ct = default)
    {
        try
        {
            dbContext.Urls.Update(url);
            await dbContext.SaveChangesAsync(ct);
            return url;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error updating url");
            return null;
        }
    }

    public async Task<Url?> DeleteUrlAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var url = await dbContext.Urls.FirstOrDefaultAsync(url => url.Id == id, ct);
            if (url == null) return url;

            dbContext.Urls.Remove(url);
            await dbContext.SaveChangesAsync(ct);
            return url;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error deleting url");
            return null;
        }
    }

    public async Task<Url?> GetUrlAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Urls.FirstOrDefaultAsync(url => url.Id == id, ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Could not get Url");
            return null;
        }
    }

    public async Task<Url?> GetUrlByShortLinkAsync(string shortLink, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Urls.FirstOrDefaultAsync(url => url.ShortLink == shortLink, ct);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Could not get Url");
            return null;
        }
    }

    public async Task<PagedResult<Url>> GetUrlsAsync(Filter? filter = null, CancellationToken ct = default)
    {
        if (filter == null)
            return new PagedResult<Url>
            {
                Items = await dbContext.Urls.ToListAsync(ct),
                TotalItems = await dbContext.Urls.CountAsync(ct),
                CurrentPage = -1,
                TotalPages = -1
            };

        var query = QueryHelper.ApplyFilter(dbContext.Urls.AsQueryable(), filter);

        var totalItems = await query.CountAsync(ct);

        if (filter.StartPage == -1)
        {
            var allItems = await query.ToListAsync(ct);
            return new PagedResult<Url>
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

        return new PagedResult<Url>
        {
            Items = items,
            TotalItems = totalItems,
            CurrentPage = filter.StartPage,
            TotalPages = totalPages
        };
    }
}