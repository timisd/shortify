using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class AddUrlEndpoint(ILogger<AddUrlEndpoint> logger, IUrlRepository urlRepo, UrlGenerator urlGenerator)
    : Endpoint<AddUrlRequest, AddUrlResponse>
{
    public override void Configure()
    {
        Post("api/urls");
    }

    [Authorize]
    public override async Task HandleAsync(AddUrlRequest req, CancellationToken ct)
    {
        logger.LogDebug("Handling add URL request for user: {UserId}", User.FindFirstValue(ClaimTypes.Sid));

        var userId = User.FindFirstValue(ClaimTypes.Sid);
        if (userId == null || !Guid.TryParse(userId, out var userGuid))
        {
            logger.LogDebug("User not found or invalid user ID.");
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "User not found"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (!isAdmin && req.ShortLink != null)
        {
            logger.LogDebug("Non-admin user attempted to specify a custom short link.");
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "Only admins can specify a custom short link"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var shortenedUrl =
            await urlRepo.AddUrlAsync(req.ToUrl(urlGenerator, User.FindFirstValue(ClaimTypes.Email)), ct);

        if (shortenedUrl == null)
        {
            logger.LogDebug("Error adding URL.");
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "Error adding url"
            }, StatusCodes.Status400BadRequest, ct);
        }
        else
        {
            logger.LogDebug("URL added successfully.");
            await SendAsync(shortenedUrl.ToAddUrlResponse(), StatusCodes.Status201Created, ct);
        }
    }
}