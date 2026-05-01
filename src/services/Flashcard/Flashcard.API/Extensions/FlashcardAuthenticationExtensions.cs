using System.Text;
using Flashcard.API.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Flashcard.API.Extensions;

public static class FlashcardAuthenticationExtensions
{
    private const string GatewayOrJwtScheme = "GatewayOrJwt";

    public static IServiceCollection AddFlashcardAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        var signingKeyMaterial = config["Jwt:SigningKey"];
        var jwtConfigured = !string.IsNullOrWhiteSpace(issuer)
            && !string.IsNullOrWhiteSpace(audience)
            && !string.IsNullOrWhiteSpace(signingKeyMaterial);

        if (!jwtConfigured)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = GatewayHeaderAuthenticationDefaults.SchemeName;
                    options.DefaultChallengeScheme = GatewayHeaderAuthenticationDefaults.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, GatewayHeaderAuthenticationHandler>(
                    GatewayHeaderAuthenticationDefaults.SchemeName,
                    _ => { });

            return services;
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyMaterial!));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = GatewayOrJwtScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddPolicyScheme(GatewayOrJwtScheme, GatewayOrJwtScheme, policyOptions =>
            {
                policyOptions.ForwardDefaultSelector = context =>
                {
                    var authorization = context.Request.Headers.Authorization.FirstOrDefault();
                    if (!string.IsNullOrEmpty(authorization)
                        && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }

                    return GatewayHeaderAuthenticationDefaults.SchemeName;
                };
            })
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
            })
            .AddScheme<AuthenticationSchemeOptions, GatewayHeaderAuthenticationHandler>(
                GatewayHeaderAuthenticationDefaults.SchemeName,
                _ => { });

        return services;
    }
}
