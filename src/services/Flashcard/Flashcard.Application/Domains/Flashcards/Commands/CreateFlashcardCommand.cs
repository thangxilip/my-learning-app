using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Commands;

public record CreateFlashcardCommand(
    Guid UserId,
    Guid DeckId,
    string Front,
    string Back,
    FsrsCardDto Card) : IRequest<FlashcardDto>;

public class CreateFlashcardCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateFlashcardCommand, FlashcardDto>
{
    public async Task<FlashcardDto> Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        var card = request.Card;
        var flashcard = Domain.Entities.Flashcard.Create(
            request.UserId,
            request.DeckId,
            request.Front,
            request.Back,
            card.Due,
            card.Stability,
            card.Difficulty,
            card.ElapsedDays,
            card.ScheduledDays,
            card.Reps,
            card.Lapses,
            card.State.ToFsrsState(),
            card.LastReview);

        await unitOfWork.Flashcards.AddAsync(flashcard, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return flashcard.ToDto();
    }
}
