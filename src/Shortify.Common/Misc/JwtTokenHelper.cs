using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using Shortify.Common.Models;

namespace Shortify.Common.Misc;

public class JwtTokenHelper(IOptions<GeneralSettings> conf)
{
    public string CreateToken(User user)
    {
        var jwt = JwtBearer.CreateToken(options =>
        {
            options.SigningKey = conf.Value.EncryptionKey;
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Email));
            if (user.IsAdmin)
                options.User.Roles.Add("admin");
            options.ExpireAt = DateTime.UtcNow.AddDays(7);
        });

        return jwt;
    }
}