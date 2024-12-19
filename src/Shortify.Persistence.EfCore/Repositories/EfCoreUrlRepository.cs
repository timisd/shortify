using Shortify.Common.Models;
using Shortify.Persistence.Models;

namespace Shortify.Persistence.EfCore.Repositories;

public class EfCoreUrlRepository : IUrlRepository
{
    public Task<Url> AddUrlAsync(Url url, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Url> DeleteUrlAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Url> GetUrlAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Url> GetUrlByShortLinkAsync(string shortLink, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<Url>> GetUrlsAsync(Filter filter, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}