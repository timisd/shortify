namespace Shortify.Common.Models;

public class GeneralSettings
{
    public required string EncryptionKey { get; init; }
    public required string EncryptionSalt { get; init; }
}