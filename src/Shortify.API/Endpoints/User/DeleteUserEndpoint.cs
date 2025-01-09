using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.User;

public class DeleteUserEndpoint(ILogger<DeleteUserEndpoint> logger, IUserRepository userRepo) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("api/users/{id}");
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

        logger.LogDebug("Handling delete user request for id: {Id}", id);
        var user = await userRepo.DeleteUserAsync(guid, ct);
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (user == null || (user.Id.ToString() != userId && !isAdmin))
        {
            logger.LogDebug("User not found or user unauthorized for id: {Id}", id);
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "User not found"
            }, StatusCodes.Status404NotFound, ct);
        }
        else
        {
            logger.LogDebug("User deleted successfully for id: {Id}", id);
            await SendNoContentAsync(ct);
        }
    }
}