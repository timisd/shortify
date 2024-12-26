using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.API.Contracts;
using Shortify.API.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;
using Shortify.Persistence.Models;

namespace Shortify.API.Endpoints.User;

public class GetUsersEndpoint(IUserRepository userRepo) : Endpoint<Filter, PagedResult<GetUserResponse>>
{
    public override void Configure()
    {
        Get("api/users");
    }

    [Authorize]
    public override async Task HandleAsync(Filter filter, CancellationToken ct)
    {
        PagedResult<GetUserResponse> result;

        var isDefaultFilter = filter is { StartPage: -1, ItemsPerPage: -1 } &&
                              string.IsNullOrEmpty(filter.OrderBy) &&
                              filter.FilterExpressions.Count == 0;
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        if (userId == null)
        {
            await SendAsync(null!, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var isAdmin = User.FindFirstValue(ClaimTypes.Role) == RolesEnum.Admin.ToFriendlyString();

        if (isDefaultFilter && isAdmin)
        {
            result = (await userRepo.GetUsersAsync(null, ct)).ToGetUserResponsePagedResult();
        }
        else
        {
            if (!isAdmin)
                filter.FilterExpressions.Add(new FilterExpression
                {
                    PropertyName = "Id",
                    Operator = FilterOperator.Equal,
                    Value = userId
                });
            result = (await userRepo.GetUsersAsync(filter, ct)).ToGetUserResponsePagedResult();
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}