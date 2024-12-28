namespace Shortify.Common.Contracts.Requests;

public record AddUrlRequest(string OriginalLink, string? ShortLink);