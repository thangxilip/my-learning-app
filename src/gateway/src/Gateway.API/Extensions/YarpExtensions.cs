namespace Gateway.API.Extensions;

public static class YarpExtensions
{
    public static IServiceCollection AddYarpProxy(this IServiceCollection services, IConfiguration config)
    {
        services.AddReverseProxy()
            .LoadFromConfig(config.GetSection("ReverseProxy"));

        return services;
    }
}