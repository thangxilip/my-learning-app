using MediatR;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;

namespace Todo.Application.Domains.TodoLists.Queries;

public class GetAllTodoListQuery : IRequest<List<TodoList>>;

public class GetAllTodoListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTodoListQuery, List<TodoList>>
{
    public async Task<List<TodoList>> Handle(GetAllTodoListQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.TodoLists.ListAsync(cancellationToken);

        return result.ToList();
    }
}