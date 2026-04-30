using Flashcard.API.Contracts;
using Flashcard.Application.Contracts;
using Flashcard.Application.Domains.Decks.Commands;
using Flashcard.Application.Domains.Decks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Flashcard.API.Controllers;

[Route("api/v1/decks")]
public class DecksController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DeckDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListDecksQuery(UserId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{deckId:guid}")]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid deckId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeckByIdQuery(UserId, deckId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDeckRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateDeckCommand(UserId, request.Name, request.Description, request.SortOrder),
            cancellationToken);

        return CreatedAtAction(nameof(GetAsync), new { deckId = result.Id }, result);
    }

    [HttpPut("{deckId:guid}")]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid deckId,
        [FromBody] UpdateDeckRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateDeckCommand(UserId, deckId, request.Name, request.Description, request.SortOrder),
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{deckId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid deckId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteDeckCommand(UserId, deckId), cancellationToken);
        return NoContent();
    }
}
