using Flashcard.API.Contracts;
using Flashcard.Application.Contracts;
using Flashcard.Application.Domains.Flashcards.Commands;
using Flashcard.Application.Domains.Flashcards.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Flashcard.API.Controllers;

[Route("api/v1")]
public class FlashcardsController(MediatR.IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("decks/{deckId:guid}/flashcards")]
    [ProducesResponseType(typeof(List<FlashcardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListByDeckAsync([FromRoute] Guid deckId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListDeckFlashcardsQuery(UserId, deckId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("decks/{deckId:guid}/flashcards/due")]
    [ProducesResponseType(typeof(List<FlashcardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListDueAsync(
        [FromRoute] Guid deckId,
        [FromQuery] DateTime? before,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new ListDueFlashcardsQuery(UserId, deckId, before ?? DateTime.UtcNow),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("decks/{deckId:guid}/flashcards")]
    [ProducesResponseType(typeof(FlashcardDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid deckId,
        [FromBody] CreateFlashcardRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateFlashcardCommand(UserId, deckId, request.Front, request.Back, request.Card),
            cancellationToken);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
    }

    [HttpGet("flashcards/{id:guid}")]
    [ProducesResponseType(typeof(FlashcardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetFlashcardByIdQuery(UserId, id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("flashcards/{id:guid}")]
    [ProducesResponseType(typeof(FlashcardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateContentAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateFlashcardContentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateFlashcardContentCommand(UserId, id, request.Front, request.Back),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("flashcards/{id:guid}/schedule")]
    [ProducesResponseType(typeof(FlashcardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateScheduleAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateFlashcardScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateFlashcardScheduleCommand(UserId, id, request.Card, request.ExpectedRowVersion),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("flashcards/{id:guid}/reviews")]
    [ProducesResponseType(typeof(ReviewSyncResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitReviewAsync(
        [FromRoute] Guid id,
        [FromBody] SubmitFlashcardReviewRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new SubmitFlashcardReviewCommand(
                UserId,
                id,
                request.Card,
                request.Log,
                request.ExpectedRowVersion,
                request.ClientMutationId),
            cancellationToken);

        return Ok(result);
    }
}
