using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Common.Models;

namespace Shortify.API.Contracts;

public static class ContractMappings
{
    public static Url MapToUrl(this AddUrlRequest req, UrlGenerator urlGenerator, Guid userId)
    {
        return new Url
        {
            Id = Guid.NewGuid(),
            OriginalLink = req.OriginalLink,
            ShortLink = req.ShortLink ?? urlGenerator.GenerateHash(req.OriginalLink),
            Visits = 0,
            UserId = userId,
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

    public static User MapToUser(this RegisterRequest req, PasswordHelper passwordHelper)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email,
            PasswordHash = passwordHelper.ComputeSha256Hash(req.Password)
        };
    }

    public static RegisterResponse MapToRegisterResponse(this User user)
    {
        return new RegisterResponse
        {
            Success = true,
            Id = user.Id,
            Email = user.Email
        };
    }
}