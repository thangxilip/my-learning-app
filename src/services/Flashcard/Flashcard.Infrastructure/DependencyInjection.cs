using AuthService.Domain.Configurations;
using Flashcard.Domain.Repositories;
using Flashcard.Domain.Repositories.Base;
using Flashcard.Infrastructure.Data;
using Flashcard.Infrastructure.Repositories;
using Flashcard.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Flashcard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFlashcardInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appConfig = configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(appConfig.ConnectionStrings.DefaultConnection);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        var npgsqlBuilderFunc = new Action<NpgsqlDbContextOptionsBuilder>(
            builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", "dev"));

        services.AddDbContext<FlashcardDbContext>(options => options.UseNpgsql(dataSource, npgsqlBuilderFunc));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<ICardReviewLogRepository, CardReviewLogRepository>();

        return services;
    }
}
