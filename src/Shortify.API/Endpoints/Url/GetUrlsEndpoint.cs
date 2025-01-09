using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Models;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class GetUrlsEndpoint(ILogger<GetUrlsEndpoint> logger, IUrlRepository urlRepo)
    : Endpoint<Filter, PagedResult<GetUrlResponse>>
{
    public override void Configure()
    {
        Get("api/urls");
    }

    [Authorize]
    public override async Task HandleAsync(Filter filter, CancellationToken ct)
    {
        logger.LogDebug("Handling get URLs request with filter: {Filter}", filter);

        PagedResult<GetUrlResponse> result;

        var isDefaultFilter = filter is { StartPage: -1, ItemsPerPage: -1 } &&
                              string.IsNullOrEmpty(filter.OrderBy) &&
                              filter.FilterExpressions.Count == 0;
        var userMail = User.FindFirstValue(ClaimTypes.Email);
        if (userMail == null)
        {
            logger.LogDebug("User email not found.");
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
                    PropertyName = nameof(Common.Models.Url.UserMail),
                    Operator = FilterOperator.Equal,
                    Value = userMail
                });
            result = (await urlRepo.GetUrlsAsync(filter, ct)).ToGetUrlResponsePagedResult();
        }

        if (!isAdmin)
        {
            var list = result.Items.ToList();
            foreach (var item in list) item.UserMail = null;
            result.Items = list.AsEnumerable();
        }

        logger.LogDebug("URLs retrieved successfully.");
        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}