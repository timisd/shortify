using FastEndpoints;
using Shortify.API.Contracts.Response;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Url;

public class DeleteUrlEndpoint(IUrlRepository urlRepo) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("api/urls/{id}");
        AllowAnonymous();
    }

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
        if (url == null)
            await SendAsync(new GetUrlResponse
            {
                Success = false,
                Message = "Url not found"
            }, StatusCodes.Status404NotFound, ct);
        else
            await SendNoContentAsync(ct);
    }
}