using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class GetUrlsEndpoint(IUrlRepository urlRepo) : Endpoint<Filter, PagedResult<GetUrlResponse>>
{
    public override void Configure()
    {
        Get("api/urls");
    }

    [Authorize]
    public override async Task HandleAsync(Filter filter, CancellationToken ct)
    {
        PagedResult<GetUrlResponse> result;

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
            result = (await urlRepo.GetUrlsAsync(null, ct)).ToGetUrlResponsePagedResult();
        }
        else
        {
            if (!isAdmin)
                filter.FilterExpressions.Add(new FilterExpression
                {
                    PropertyName = nameof(Common.Models.Url.UserId),
                    Operator = FilterOperator.Equal,
                    Value = userId
                });
            result = (await urlRepo.GetUrlsAsync(filter, ct)).ToGetUrlResponsePagedResult();
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}