namespace Shortify.Common.Models;

public class ApiSettings
{
    public required string EncryptionKey { get; init; }
    public required string EncryptionSalt { get; init; }
}