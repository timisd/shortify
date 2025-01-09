using FastEndpoints;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;
using Shortify.Persistence;

namespace Shortify.API.Endpoints.Auth;

public class LoginEndpoint(ILogger<LoginEndpoint> logger, IUserRepository userRepo, JwtTokenHelper jwtTokenHelper)
    : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        logger.LogDebug("Handling login request for email: {Email}", req.Email);

        var user = await userRepo.GetUserByLoginAsync(req.Email, req.Password, ct);
        if (user == null)
        {
            logger.LogDebug("No user found with email: {Email}", req.Email);
            await SendAsync(new LoginResponse
            {
                Success = false,
                Message = "No user found with this credentials"
            }, StatusCodes.Status404NotFound, ct);
            return;
        }

        var token = jwtTokenHelper.CreateToken(user);
        logger.LogDebug("Token created for user: {Email}", req.Email);
        await SendAsync(new LoginResponse
        {
            Success = true,
            Token = token
        }, StatusCodes.Status200OK, ct);
    }
}