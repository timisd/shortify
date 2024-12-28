namespace Shortify.Common.Contracts.Response;

public class GetUserResponse : BaseResponse
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedAt { get; set; }
}