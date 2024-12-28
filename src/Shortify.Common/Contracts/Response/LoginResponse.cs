namespace Shortify.Common.Contracts.Response;

public class LoginResponse : BaseResponse
{
    public string Token { get; set; } = string.Empty;
}