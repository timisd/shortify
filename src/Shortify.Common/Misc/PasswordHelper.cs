using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Shortify.Common.Models;

namespace Shortify.Common.Misc;

public class PasswordHelper(IOptions<GeneralSettings> config)
{
    private readonly Random _random = new();

    public string GenerateToken()
    {
        return RandomString(64);
    }

    public string ComputeSha256Hash(string rawData)
    {
        var bytes = Encoding.UTF8.GetBytes(rawData + config.Value.EncryptionSalt);
        bytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(bytes);
    }

    private string RandomString(int length)
    {
        return new string(Enumerable.Repeat(CharSet.Alphanumeric, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}