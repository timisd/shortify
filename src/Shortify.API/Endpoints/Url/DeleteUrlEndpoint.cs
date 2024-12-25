using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.API.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class DeleteUrlEndpoint(IUrlRepository urlRepo) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("api/urls/{id}");
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


        var url = await urlRepo.DeleteUrlAsync(guid, ct);
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (url == null || (url.UserId.ToString() != userId && !isAdmin))
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Url not found"
            }, StatusCodes.Status404NotFound, ct);
        else
            await SendNoContentAsync(ct);
    }
}