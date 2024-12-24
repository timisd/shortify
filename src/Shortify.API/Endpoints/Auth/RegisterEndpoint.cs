using FastEndpoints;
using Shortify.API.Contracts;
using Shortify.API.Contracts.Requests;
using Shortify.API.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Auth;

public class RegisterEndpoint(IUserRepository userRepo, PasswordHelper passwordHelper)
    : Endpoint<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post("/api/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var user = await userRepo.GetUserByEmailAsync(req.Email, ct);
        if (user != null)
        {
            await SendAsync(new RegisterResponse
            {
                Success = false,
                Message = "User already exists"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var newUser = req.MapToUser(passwordHelper);
        var result = await userRepo.AddUserAsync(newUser, ct);
        if (result == null)
        {
            await SendAsync(new RegisterResponse
            {
                Success = false,
                Message = "Error adding user"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result.MapToRegisterResponse(), StatusCodes.Status201Created, ct);
    }
}