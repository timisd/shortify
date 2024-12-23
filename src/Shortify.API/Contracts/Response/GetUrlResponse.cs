namespace Shortify.API.Contracts.Response;

public class GetUrlResponse : BaseResponse
{
    public string? OriginalLink { get; set; }
    public string? ShortLink { get; set; }
    public int Visits { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UserId { get; set; }
}