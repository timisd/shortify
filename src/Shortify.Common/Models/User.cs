namespace Shortify.Common.Models;

public class User : Entity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolesEnum Role { get; set; }

    /*public List<Claim> ToClaims()
    {
        return
        [
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Role, Role.ToFriendlyString())
        ];
    }*/
}