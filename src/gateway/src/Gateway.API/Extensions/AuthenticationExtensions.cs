using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        var signingKeyMaterial = config["Jwt:SigningKey"];

        if (string.IsNullOrWhiteSpace(issuer)
            || string.IsNullOrWhiteSpace(audience)
            || string.IsNullOrWhiteSpace(signingKeyMaterial))
        {
            throw new InvalidOperationException(
                "Jwt:Issuer, Jwt:Audience, and Jwt:SigningKey must be configured (same values as the Auth service issuer).");
        }

        // Must match AuthService JwtTokenService: SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey))
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyMaterial));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                };
            });

        return services;
    }
}
