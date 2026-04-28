using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories.Base;

namespace Flashcard.Domain.Repositories;

public interface IDeckRepository : IRepository<Deck>
{
    Task<IReadOnlyList<Deck>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Deck?> GetByIdForUserAsync(Guid userId, Guid deckId, CancellationToken cancellationToken = default);
}
