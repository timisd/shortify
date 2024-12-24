using System.Security.Cryptography;
using System.Text;

namespace Shortify.Common.Misc;

public class UrlGenerator
{
    public string GenerateHash(string input)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        var shortUrl = new StringBuilder();
        for (var i = 0; i < 6; i++)
            shortUrl.Append(CharSet.Alphanumeric[hashBytes[i] % CharSet.Alphanumeric.Length]);

        return shortUrl.ToString();
    }
}