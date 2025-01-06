using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shortify.Common.Models;

namespace Shortify.Common.Misc;

public class JwtTokenHelper(IOptions<ApiSettings> conf)
{
    public string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ApiSettings__EncryptionKey") ??
                                         conf.Value.EncryptionKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToFriendlyString())
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Audience = "Shortify",
            Issuer = "Shortify",
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}