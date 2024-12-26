using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Common.Models;
using Shortify.Persistence.Models;

namespace Shortify.API.Contracts;

public static class ContractMappings
{
    public static Url ToUrl(this AddUrlRequest req, UrlGenerator urlGenerator, Guid userId)
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

    public static AddUrlResponse ToAddUrlResponse(this Url url)
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

    public static GetUrlResponse ToGetUrlResponse(this Url url)
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

    public static User ToUser(this RegisterRequest req, PasswordHelper passwordHelper)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email,
            PasswordHash = passwordHelper.ComputeSha256Hash(req.Password)
        };
    }

    public static RegisterResponse ToRegisterResponse(this User user)
    {
        return new RegisterResponse
        {
            Success = true,
            Id = user.Id,
            Email = user.Email
        };
    }

    public static GetUserResponse ToGetUserResponse(this User user)
    {
        return new GetUserResponse
        {
            Success = true,
            Id = user.Id,
            Email = user.Email,
            Role = user.Role.ToFriendlyString()
        };
    }

    public static PagedResult<GetUrlResponse> ToGetUrlResponsePagedResult(this PagedResult<Url> pagedResult)
    {
        return new PagedResult<GetUrlResponse>
        {
            Items = pagedResult.Items.Select(url => url.ToGetUrlResponse()).ToList(),
            TotalItems = pagedResult.TotalItems,
            TotalPages = pagedResult.TotalPages,
            CurrentPage = pagedResult.CurrentPage
        };
    }

    public static PagedResult<GetUserResponse> ToGetUserResponsePagedResult(this PagedResult<User> pagedResult)
    {
        return new PagedResult<GetUserResponse>
        {
            Items = pagedResult.Items.Select(user => user.ToGetUserResponse()).ToList(),
            TotalItems = pagedResult.TotalItems,
            TotalPages = pagedResult.TotalPages,
            CurrentPage = pagedResult.CurrentPage
        };
    }
}