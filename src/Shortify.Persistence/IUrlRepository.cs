using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;

namespace Shortify.Persistence;

public interface IUrlRepository
{
    Task<Url?> AddUrlAsync(Url url, CancellationToken ct = default);
    Task<Url?> UpdateUrlAsync(Url url, CancellationToken ct = default);
    Task<Url?> DeleteUrlAsync(Guid id, CancellationToken ct = default);
    Task<Url?> GetUrlAsync(Guid id, CancellationToken ct = default);
    Task<Url?> GetUrlByShortLinkAsync(string shortLink, CancellationToken ct = default);
    Task<PagedResult<Url>> GetUrlsAsync(Filter? filter, CancellationToken ct = default);
}