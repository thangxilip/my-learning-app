using Flashcard.Application.Common.Exceptions;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Queries;

public record ListDeckFlashcardsQuery(Guid UserId, Guid DeckId) : IRequest<List<Domain.Entities.Flashcard>>;

public class ListDeckFlashcardsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ListDeckFlashcardsQuery, List<Domain.Entities.Flashcard>>
{
    public async Task<List<Domain.Entities.Flashcard>> Handle(ListDeckFlashcardsQuery request, CancellationToken cancellationToken)
    {
        var deckExists = await unitOfWork.Decks.ExistsAsync(
            x => x.Id == request.DeckId && x.UserId == request.UserId,
            cancellationToken);

        if (!deckExists)
        {
            throw new NotFoundException("Deck not found.");
        }

        var flashcards = await unitOfWork.Flashcards.ListByDeckAsync(request.UserId, request.DeckId, cancellationToken);
        return flashcards.ToList();
    }
}
