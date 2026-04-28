using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using Flashcard.Infrastructure.Data;
using Flashcard.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Flashcard.Infrastructure.Repositories;

public class DeckRepository(FlashcardDbContext context) : Repository<Deck>(context), IDeckRepository
{
    public async Task<IReadOnlyList<Deck>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Deck?> GetByIdForUserAsync(Guid userId, Guid deckId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == deckId && x.UserId == userId, cancellationToken)
            .ConfigureAwait(false);
    }
}
