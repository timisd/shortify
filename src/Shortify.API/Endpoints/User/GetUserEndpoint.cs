using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.API.Contracts;
using Shortify.API.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.User;

public class GetUserEndpoint(IUserRepository userRepo) : EndpointWithoutRequest<GetUserResponse>
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
            await SendAsync(new GetUserResponse
            {
                Success = false,
                Message = "Invalid id"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var user = await userRepo.GetUserAsync(guid, ct);
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();
        if (user == null || (user.Id.ToString() != userId && !isAdmin))
            await SendAsync(new GetUserResponse
            {
                Success = false,
                Message = "User not found"
            }, StatusCodes.Status404NotFound, ct);
        else
            await SendAsync(user.ToGetUserResponse(), StatusCodes.Status200OK, ct);
    }
}