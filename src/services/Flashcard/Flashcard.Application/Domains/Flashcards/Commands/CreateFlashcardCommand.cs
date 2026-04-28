using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Enums;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Commands;

public record CreateFlashcardCommand(
    Guid UserId,
    Guid DeckId,
    string Front,
    string Back,
    FsrsCardDto Card) : IRequest<Domain.Entities.Flashcard>;

public class CreateFlashcardCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateFlashcardCommand, Domain.Entities.Flashcard>
{
    public async Task<Domain.Entities.Flashcard> Handle(CreateFlashcardCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        var card = request.Card;
        
        var flashcard = new Domain.Entities.Flashcard
        {
            UserId = request.UserId,
            DeckId = request.DeckId,
            Front = request.Front,
            Back = request.Back,
            Due = card.Due,
            Stability = card.Stability,
            Difficulty = card.Difficulty,
            ElapsedDays = card.ElapsedDays,
            ScheduledDays = card.ScheduledDays,
            Reps = card.Reps,
            Lapses = card.Lapses,
            State = (FsrsState)card.State,
            LastReview = card.LastReview,
        };

        await unitOfWork.Flashcards.AddAsync(flashcard, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return flashcard;
    }
}
