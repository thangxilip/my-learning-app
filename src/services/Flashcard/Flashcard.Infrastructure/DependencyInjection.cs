using Flashcard.Domain.Repositories;
using Flashcard.Infrastructure.Data;
using Flashcard.Infrastructure.Repositories;
using Flashcard.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFlashcardInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=flashcard.db";

        services.AddDbContext<FlashcardDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<ICardReviewLogRepository, CardReviewLogRepository>();

        return services;
    }
}
