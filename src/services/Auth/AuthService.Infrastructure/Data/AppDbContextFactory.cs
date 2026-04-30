using AuthService.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace AuthService.Infrastructure.Data;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(envName))
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var environment = envName ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<AppDbContextFactory>(optional: true)
            .Build();
        
        var appConfig = configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(appConfig.ConnectionStrings.DefaultConnection);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        var npgsqlBuilderFunc = new Action<NpgsqlDbContextOptionsBuilder>(builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", "dev"));
        var builder = new DbContextOptionsBuilder<AuthDbContext>();
        builder.UseNpgsql(dataSource, npgsqlBuilderFunc);

        return new AuthDbContext(builder.Options);
    }
}
