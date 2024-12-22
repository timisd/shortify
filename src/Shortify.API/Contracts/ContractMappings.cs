using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Common.Models;

namespace Shortify.API.Contracts;

public static class ContractMappings
{
    public static Url MapToUrl(this AddUrlRequest req)
    {
        return new Url
        {
            Id = Guid.NewGuid(),
            OriginalLink = req.OriginalLink,
            ShortLink = req.ShortLink ?? UrlGenerator.GenerateHash(req.OriginalLink),
            Visits = 0,
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public static AddUrlResponse MapToResponse(this Url url)
    {
        return new AddUrlResponse
        {
            OriginalLink = url.OriginalLink,
            ShortLink = url.ShortLink,
            Visits = url.Visits,
            CreatedAt = url.CreatedAt
        };
    }
}