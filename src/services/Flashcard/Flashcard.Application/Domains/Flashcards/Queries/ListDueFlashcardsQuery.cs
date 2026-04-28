using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Queries;

public record ListDueFlashcardsQuery(Guid UserId, Guid DeckId, DateTime Before) : IRequest<List<FlashcardDto>>;

public class ListDueFlashcardsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ListDueFlashcardsQuery, List<FlashcardDto>>
{
    public async Task<List<FlashcardDto>> Handle(ListDueFlashcardsQuery request, CancellationToken cancellationToken)
    {
        var deckExists = await unitOfWork.Decks.ExistsAsync(
            x => x.Id == request.DeckId && x.UserId == request.UserId,
            cancellationToken);

        if (!deckExists)
        {
            throw new NotFoundException("Deck not found.");
        }

        var flashcards = await unitOfWork.Flashcards.ListDueByDeckAsync(
            request.UserId,
            request.DeckId,
            request.Before,
            cancellationToken);

        return flashcards.Select(x => x.ToDto()).ToList();
    }
}
