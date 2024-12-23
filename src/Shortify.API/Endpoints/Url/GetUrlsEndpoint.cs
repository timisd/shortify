using FastEndpoints;
using Shortify.Persistence;
using Shortify.Persistence.Models;

namespace Shortify.API.Endpoints.Url;

public class GetUrlsEndpoint(IUrlRepository urlRepo) : Endpoint<Filter, PagedResult<Common.Models.Url>>
{
    public override void Configure()
    {
        Get("/api/urls");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Filter filter, CancellationToken ct)
    {
        PagedResult<Common.Models.Url> result;

        var isDefaultFilter = filter is { StartPage: -1, ItemsPerPage: -1 } &&
                              string.IsNullOrEmpty(filter.OrderBy) &&
                              !filter.FilterExpressions.Any();

        if (isDefaultFilter)
            result = await urlRepo.GetUrlsAsync(null, ct);
        else
            result = await urlRepo.GetUrlsAsync(filter, ct);

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}