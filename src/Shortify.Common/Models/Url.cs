namespace Shortify.Common.Models;

public class Url : Entity
{
    public string OriginalLink { get; set; } = string.Empty;
    public string ShortLink { get; set; } = string.Empty;
    public int Visits { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserMail { get; set; } = string.Empty;
}