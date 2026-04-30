using AuthService.Application.Security;
using AuthService.Domain.Configurations;
using AuthService.Domain.Repositories;
using AuthService.Domain.Repositories.Base;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repositories.Base;
using AuthService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appConfig = configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(appConfig.ConnectionStrings.DefaultConnection);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        var npgsqlBuilderFunc = new Action<NpgsqlDbContextOptionsBuilder>(
            builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", "dev"));

        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(dataSource, npgsqlBuilderFunc));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
