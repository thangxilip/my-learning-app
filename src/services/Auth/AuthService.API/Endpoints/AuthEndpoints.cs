using AuthService.API.Contracts;
using AuthService.API.Services;
using AuthService.Application.Security;
using AuthService.Domain.Repositories;

namespace AuthService.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/auth")
            .WithTags("Auth");

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return endpoints;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.Problem(
                title: "Validation error",
                detail: "Email and password are required.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var email = request.Email.Trim().ToLowerInvariant();

        var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
        {
            return Results.Problem(
                title: "Not found",
                detail: "No user found with this email.",
                statusCode: StatusCodes.Status404NotFound);
        }

        if (!user.IsActive)
        {
            return Results.Problem(
                title: "Forbidden",
                detail: "Account is inactive.",
                statusCode: StatusCodes.Status403Forbidden);
        }

        if (!passwordHasher.VerifyPassword(request.Password, user.Salt, user.PasswordHash))
        {
            return Results.Problem(
                title: "Unauthorized",
                detail: "Incorrect password.",
                statusCode: StatusCodes.Status401Unauthorized);
        }

        var tokens = jwtTokenService.CreateTokens(user.Id, user.Email, user.FirstName, user.LastName);

        return Results.Ok(new LoginResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAtUtc));
    }
}
