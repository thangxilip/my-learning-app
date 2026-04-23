using Todo.Domain.Entities;
using Todo.Domain.Repositories;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.Repositories.Base;

namespace Todo.Infrastructure.Repositories;

public class TodoListRepository(AppDbContext context) : Repository<TodoList>(context), ITodoListRepository
{
    
}