namespace Shortify.Common.Models;

public class User : Entity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolesEnum Role { get; set; }
    public DateTime CreatedAt { get; set; }
}