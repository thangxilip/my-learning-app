using Todo.Domain.Entities;
using Todo.Domain.Repositories.Base;

namespace Todo.Domain.Repositories;

public interface IUnitOfWork
{
    ITodoListRepository TodoLists { get; }

    IRepository<TodoItem> TodoItems { get; }

    IRepository<Tag> Tags { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
