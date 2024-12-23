using FastEndpoints;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Redirect;

public class RedirectEndpoint(IUrlRepository urlRepo) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("{shortLink}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var shortLink = Route<string>("shortLink");
        if (string.IsNullOrEmpty(shortLink))
        {
            await SendAsync(null, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var url = await urlRepo.GetUrlByShortLinkAsync(shortLink, ct);
        if (url == null)
        {
            await SendAsync(null, StatusCodes.Status404NotFound, ct);
            return;
        }

        url.Visits++;
        await urlRepo.UpdateUrlAsync(url, ct);
        var originalUrl = url.OriginalLink;
        if (string.IsNullOrEmpty(originalUrl))
        {
            await SendAsync(null, StatusCodes.Status404NotFound, ct);
            return;
        }

        if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
            originalUrl = "http://" + originalUrl;

        await SendRedirectAsync(originalUrl, true, true);
    }
}