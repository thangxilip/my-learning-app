using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.API.Extensions;

public static class YarpExtensions
{
    public static IServiceCollection AddYarpProxy(this IServiceCollection services, IConfiguration config)
    {
        services.AddReverseProxy()
            .LoadFromConfig(config.GetSection("ReverseProxy"))
            .AddTransforms(builder =>
            {
                builder.AddRequestTransform(ctx =>
                {
                    ctx.ProxyRequest.Headers.Remove("Authorization");

                    var user = ctx.HttpContext.User;
                    if (user?.Identity?.IsAuthenticated != true)
                    {
                        return ValueTask.CompletedTask;
                    }

                    var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                              ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(sub))
                    {
                        ctx.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Id", sub);
                    }

                    var email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                                ?? user.FindFirst(ClaimTypes.Email)?.Value;
                    if (!string.IsNullOrEmpty(email))
                    {
                        ctx.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Email", email);
                    }

                    return ValueTask.CompletedTask;
                });
            });

        return services;
    }
}
