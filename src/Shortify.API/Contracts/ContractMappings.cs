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

    public static AddUrlResponse MapToAddUrlResponse(this Url url)
    {
        return new AddUrlResponse
        {
            OriginalLink = url.OriginalLink,
            ShortLink = url.ShortLink,
            Visits = url.Visits,
            CreatedAt = url.CreatedAt,
            Success = true
        };
    }

    public static GetUrlResponse MapToGetUrlResponse(this Url url)
    {
        return new GetUrlResponse
        {
            OriginalLink = url.OriginalLink,
            ShortLink = url.ShortLink,
            Visits = url.Visits,
            CreatedAt = url.CreatedAt,
            UserId = url.UserId,
            Success = true
        };
    }
}