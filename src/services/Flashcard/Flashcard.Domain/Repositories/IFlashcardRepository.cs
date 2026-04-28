using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories.Base;

namespace Flashcard.Domain.Repositories;

public interface IFlashcardRepository : IRepository<Entities.Flashcard>
{
    Task<IReadOnlyList<Entities.Flashcard>> ListByDeckAsync(
        Guid userId,
        Guid deckId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Entities.Flashcard>> ListDueByDeckAsync(
        Guid userId,
        Guid deckId,
        DateTime before,
        CancellationToken cancellationToken = default);

    Task<Entities.Flashcard?> GetByIdForUserAsync(
        Guid userId,
        Guid flashcardId,
        CancellationToken cancellationToken = default);
}
