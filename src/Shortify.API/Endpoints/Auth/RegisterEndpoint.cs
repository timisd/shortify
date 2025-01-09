using FastEndpoints;
using Shortify.Common.Contracts;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Auth;

public class RegisterEndpoint(ILogger<RegisterEndpoint> logger, IUserRepository userRepo, PasswordHelper passwordHelper)
    : Endpoint<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post("api/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        logger.LogDebug("Handling register request for email: {Email}", req.Email);

        var user = await userRepo.GetUserByEmailAsync(req.Email, ct);
        if (user != null)
        {
            logger.LogDebug("User already exists with email: {Email}", req.Email);
            await SendAsync(new RegisterResponse
            {
                Success = false,
                Message = "User already exists"
            }, StatusCodes.Status409Conflict, ct);
            return;
        }

        var newUser = req.ToUser(passwordHelper);
        var result = await userRepo.AddUserAsync(newUser, ct);
        if (result == null)
        {
            logger.LogDebug("User already exists with email: {Email}", req.Email);
            await SendAsync(new RegisterResponse
            {
                Success = false,
                Message = "Error adding user"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        logger.LogDebug("User registered successfully with email: {Email}", req.Email);
        await SendAsync(result.ToRegisterResponse(), StatusCodes.Status201Created, ct);
    }
}