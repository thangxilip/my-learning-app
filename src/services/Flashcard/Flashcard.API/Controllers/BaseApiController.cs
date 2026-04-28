using Flashcard.API.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcard.API.Controllers;

[ApiController]
[Authorize]
public abstract class BaseApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator { get; } = mediator;

    protected Guid UserId => User.GetRequiredUserId();
}
