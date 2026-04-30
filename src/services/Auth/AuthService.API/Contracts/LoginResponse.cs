namespace AuthService.API.Contracts;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAtUtc);
