using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Domain.Repositories;
using Todo.Domain.Repositories.Base;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.Repositories;
using Todo.Infrastructure.Repositories.Base;

namespace Todo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTodoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new NullReferenceException("Invalid connection string");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
