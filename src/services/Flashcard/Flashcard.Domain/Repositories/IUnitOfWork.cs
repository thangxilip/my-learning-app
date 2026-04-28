using Flashcard.Domain.Repositories.Base;

namespace Flashcard.Domain.Repositories;

public interface IUnitOfWork
{
    IDeckRepository Decks { get; }
    IFlashcardRepository Flashcards { get; }
    ICardReviewLogRepository ReviewLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
