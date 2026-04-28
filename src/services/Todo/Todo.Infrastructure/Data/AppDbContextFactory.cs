using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Todo.Domain.Configurations;

namespace Todo.Infrastructure.Data;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
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
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseNpgsql(dataSource, npgsqlBuilderFunc);

        return new AppDbContext(builder.Options);
    }
}
