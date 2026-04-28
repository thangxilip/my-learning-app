using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Domains.TodoLists;
using Todo.Application.Domains.TodoLists.Commands;
using Todo.Application.Domains.TodoLists.Queries;
using Todo.Domain.Entities;

namespace Todo.API.Controllers;

public class TodoListController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all todo lists
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(List<TodoList>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var todoLists = await mediator.Send(new GetAllTodoListQuery(), cancellationToken);
        return Ok(todoLists);
    }
    
    /// <summary>
    /// Create a todo lít
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(TodoList), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync(CreateTodoListCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}