using System.Security.Claims;

namespace Shortify.Common.Models;

public class User : Entity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public RolesEnum Role { get; set; }

    public List<Claim> ToClaims()
    {
        return
        [
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Role, Role.ToFriendlyString())
        ];
    }
}