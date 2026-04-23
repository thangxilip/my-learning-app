using Todo.Domain.Entities;
using Todo.Domain.Repositories;
using Todo.Domain.Repositories.Base;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repositories.Base;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(
        AppDbContext context,
        ITodoListRepository todoLists,
        IRepository<TodoItem> todoItems,
        IRepository<Tag> tags)
    {
        _context = context;
        TodoLists = todoLists;
        TodoItems = todoItems;
        Tags = tags;
    }

    public ITodoListRepository TodoLists { get; }

    public IRepository<TodoItem> TodoItems { get; }

    public IRepository<Tag> Tags { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
