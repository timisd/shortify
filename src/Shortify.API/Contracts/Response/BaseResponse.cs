namespace Shortify.API.Contracts.Response;

public class BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}