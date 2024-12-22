using FastEndpoints;
using Shortify.API.Contracts;
using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.UrlEndpoints;

public class AddUrlEndpoint(IUrlRepository urlRepo) : Endpoint<AddUrlRequest, AddUrlResponse>
{
    public override void Configure()
    {
        Post("/api/urls");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUrlRequest req, CancellationToken ct)
    {
        var shortenedUrl = await urlRepo.AddUrlAsync(req.MapToUrl(), ct);

        if (shortenedUrl == null)
            await SendAsync(new AddUrlResponse
            {
                Success = false,
                Message = "Error adding url"
            }, StatusCodes.Status400BadRequest, ct);
        else
            await SendAsync(shortenedUrl.MapToResponse(), StatusCodes.Status201Created, ct);
    }
}