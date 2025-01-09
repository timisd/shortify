using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.User;

public class GetUserEndpoint(ILogger<GetUserEndpoint> logger, IUserRepository userRepo)
    : EndpointWithoutRequest<GetUserResponse>
{
    public override void Configure()
    {
        Get("api/users/{id}");
    }

    [Authorize]
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        var isGuid = Guid.TryParse(id, out var guid);
        if (!isGuid)
        {
            logger.LogDebug("Invalid id: {Id}", id);
            await SendAsync(new GetUserResponse
            {
                Success = false,
                Message = "Invalid id"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        logger.LogDebug("Handling get user request for id: {Id}", id);
        var user = await userRepo.GetUserAsync(guid, ct);
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (user == null || (user.Id.ToString() != userId && !isAdmin))
        {
            logger.LogDebug("User not found or user unauthorized for id: {Id}", id);
            await SendAsync(new GetUserResponse
            {
                Success = false,
                Message = "User not found"
            }, StatusCodes.Status404NotFound, ct);
        }
        else
        {
            logger.LogDebug("User retrieved successfully for id: {Id}", id);
            await SendAsync(user.ToGetUserResponse(), StatusCodes.Status200OK, ct);
        }
    }
}