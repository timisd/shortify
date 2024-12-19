using System.Security.Claims;

namespace Shortify.Common.Models;

public class User : Entity
{
    string Email { get; set; }
    string PasswordHash { get; set; }
    bool IsAdmin { get; set; }
    
    public List<Claim> ToClaims()
    {
        return
        [
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Role, IsAdmin ? "Admin" : "User")
        ];
    }
}