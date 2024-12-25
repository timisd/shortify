using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Shortify.API.Contracts;
using Shortify.API.Contracts.Response;
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
        if (url == null)
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Url not found"
            }, StatusCodes.Status404NotFound, ct);
        else
            await SendAsync(url.MapToGetUrlResponse(), StatusCodes.Status200OK, ct);
    }
}