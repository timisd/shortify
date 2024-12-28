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

public class AddUrlEndpoint(IUrlRepository urlRepo, UrlGenerator urlGenerator) : Endpoint<AddUrlRequest, AddUrlResponse>
{
    public override void Configure()
    {
        Post("api/urls");
    }

    [Authorize]
    public override async Task HandleAsync(AddUrlRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        if (userId == null || !Guid.TryParse(userId, out var userGuid))
        {
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
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "Only admins can specify a custom short link"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var shortenedUrl = await urlRepo.AddUrlAsync(req.ToUrl(urlGenerator, userGuid), ct);

        if (shortenedUrl == null)
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "Error adding url"
            }, StatusCodes.Status400BadRequest, ct);
        else
            await SendAsync(shortenedUrl.ToAddUrlResponse(), StatusCodes.Status201Created, ct);
    }
}