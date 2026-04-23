using MediatR;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;

namespace Todo.Application.Domains.TodoLists.Commands;

public record CreateTodoListCommand(string Title) : IRequest<TodoList>;

public class CreateTodoListCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTodoListCommand, TodoList>
{
    public async Task<TodoList> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = new TodoList
        {
            Name = request.Title,
        };
        await unitOfWork.TodoLists.AddAsync(todoList, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return todoList;
    }
}