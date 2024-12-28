using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class GetUrlEndpoint(IUrlRepository urlRepo) : EndpointWithoutRequest<GetUrlResponse>
{
    public override void Configure()
    {
        Get("api/urls/{id}");
    }

    [Authorize]
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        var isGuid = Guid.TryParse(id, out var guid);
        if (!isGuid)
        {
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Invalid id"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var url = await urlRepo.GetUrlAsync(guid, ct);
        var userMail = User.FindFirstValue(ClaimTypes.Email);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (url == null || (url.UserMail != userMail && !isAdmin))
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Url not found"
            }, StatusCodes.Status404NotFound, ct);
        else
            await SendAsync(url.ToGetUrlResponse(), StatusCodes.Status200OK, ct);
    }
}