namespace Shortify.Common.Contracts.Response;

public class GetUrlResponse : BaseResponse
{
    public Guid? Id { get; set; }
    public string? OriginalLink { get; set; }
    public string? ShortLink { get; set; }
    public int Visits { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserMail { get; set; }
}