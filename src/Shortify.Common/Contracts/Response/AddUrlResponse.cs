namespace Shortify.Common.Contracts.Response;

public class AddUrlResponse : BaseResponse
{
    public string? OriginalLink { get; set; }
    public string? ShortLink { get; set; }
    public int Visits { get; set; }
    public DateTime CreatedAt { get; set; }
}