using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Flashcard.API.Auth;

/// <summary>
/// Authenticates requests using identity headers set by the API gateway (<c>X-User-Id</c>, optional <c>X-User-Email</c>).
/// Trust boundary: only requests routed through the gateway should reach this service; network isolation prevents spoofing.
/// </summary>
public static class GatewayHeaderAuthenticationDefaults
{
    public const string SchemeName = "GatewayHeaders";
}

public sealed class GatewayHeaderAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-User-Id", out var userIdValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var raw = userIdValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(raw) || !Guid.TryParse(raw, out var userId))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid X-User-Id header."));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        if (Request.Headers.TryGetValue("X-User-Email", out var emailValues))
        {
            var email = emailValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
