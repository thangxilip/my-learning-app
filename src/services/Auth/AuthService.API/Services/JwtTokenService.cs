using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.API.Services;

public interface IJwtTokenService
{
    TokenPair CreateTokens(Guid userId, string email, string firstName, string lastName);
}

public sealed record TokenPair(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiresAtUtc);

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;

    public TokenPair CreateTokens(Guid userId, string email, string firstName, string lastName)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var handler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var accessExpires = now.AddMinutes(_options.AccessTokenMinutes);
        var refreshExpires = now.AddDays(_options.RefreshTokenDays);

        var accessClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("token_use", "access"),
        };

        var accessToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            accessClaims,
            expires: accessExpires,
            signingCredentials: credentials);

        var refreshClaims = new List<Claim>
        {
            new("token_use", "refresh"),
            new("user_id", userId.ToString()),
            new("first_name", firstName),
            new("last_name", lastName),
        };

        var refreshToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            refreshClaims,
            expires: refreshExpires,
            signingCredentials: credentials);

        return new TokenPair(
            handler.WriteToken(accessToken),
            handler.WriteToken(refreshToken),
            new DateTimeOffset(accessExpires, TimeSpan.Zero));
    }
}
