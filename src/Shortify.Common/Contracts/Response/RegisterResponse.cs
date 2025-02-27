namespace Shortify.Common.Contracts.Response;

public class RegisterResponse : BaseResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
}