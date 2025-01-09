using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class DeleteUrlEndpoint(ILogger<DeleteUrlEndpoint> logger, IUrlRepository urlRepo) : EndpointWithoutRequest
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
            logger.LogDebug("Invalid id: {Id}", id);
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Invalid id"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        logger.LogDebug("Handling delete URL request for id: {Id}", id);
        var url = await urlRepo.DeleteUrlAsync(guid, ct);
        var userMail = User.FindFirstValue(ClaimTypes.Email);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (url == null || (url.UserMail != userMail && !isAdmin))
        {
            logger.LogDebug("Url not found or user unauthorized for id: {Id}", id);
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Url not found"
            }, StatusCodes.Status404NotFound, ct);
        }
        else
        {
            logger.LogDebug("URL deleted successfully for id: {Id}", id);
            await SendNoContentAsync(ct);
        }
    }
}