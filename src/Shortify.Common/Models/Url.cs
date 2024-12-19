namespace Shortify.Common.Models;

public class Url : Entity
{
    string OriginalLink { get; set; }
    string ShortLink { get; set; }
    int Visits { get; set; }
    DateTime CreatedAt { get; set; }
    Guid? UserId { get; set; }
}