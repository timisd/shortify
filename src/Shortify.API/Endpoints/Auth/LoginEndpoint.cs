using FastEndpoints;
using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Auth;

public class LoginEndpoint(IUserRepository userRepo, JwtTokenHelper jwtTokenHelper)
    : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await userRepo.GetUserByLoginAsync(req.Email, req.Password, ct);
        if (user == null)
        {
            await SendAsync(new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var token = jwtTokenHelper.CreateToken(user);
        await SendAsync(new LoginResponse
        {
            Success = true,
            Token = token
        }, StatusCodes.Status200OK, ct);
    }
}