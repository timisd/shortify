namespace Shortify.API.Contracts.Requests;

public record AddUrlRequest(string OriginalLink, string? ShortLink);