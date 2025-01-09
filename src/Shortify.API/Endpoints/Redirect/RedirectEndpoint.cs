using FastEndpoints;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Redirect;

public class RedirectEndpoint(ILogger<RedirectEndpoint> logger, IUrlRepository urlRepo) : EndpointWithoutRequest
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
            logger.LogDebug("Short link is null or empty.");
            await SendAsync(null, StatusCodes.Status400BadRequest, ct);
            return;
        }

        logger.LogDebug("Handling redirect request for short link: {ShortLink}", shortLink);

        var url = await urlRepo.GetUrlByShortLinkAsync(shortLink, ct);
        if (url == null)
        {
            logger.LogDebug("No URL found for short link: {ShortLink}", shortLink);
            await SendAsync(null, StatusCodes.Status404NotFound, ct);
            return;
        }

        url.Visits++;
        await urlRepo.UpdateUrlAsync(url, ct);
        var originalUrl = url.OriginalLink;
        if (string.IsNullOrEmpty(originalUrl))
        {
            logger.LogDebug("Original URL is null or empty for short link: {ShortLink}", shortLink);
            await SendAsync(null, StatusCodes.Status404NotFound, ct);
            return;
        }

        if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
            originalUrl = "http://" + originalUrl;

        logger.LogDebug("Redirecting to original URL: {OriginalUrl}", originalUrl);
        await SendAsync(originalUrl, StatusCodes.Status200OK, ct);
    }
}