namespace Shortify.Common.Models;

public class Url : Entity
{
    public string OriginalLink { get; set; }
    public string ShortLink { get; set; }
    public int Visits { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UserId { get; set; }
}