using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFlashcardApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        return services;
    }
}
