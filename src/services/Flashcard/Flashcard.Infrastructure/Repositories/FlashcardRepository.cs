using Flashcard.Domain.Repositories;
using Flashcard.Infrastructure.Data;
using Flashcard.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Flashcard.Infrastructure.Repositories;

public class FlashcardRepository(FlashcardDbContext context)
    : Repository<Domain.Entities.Flashcard>(context), IFlashcardRepository
{
    public async Task<IReadOnlyList<Domain.Entities.Flashcard>> ListByDeckAsync(
        Guid userId,
        Guid deckId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(x => x.UserId == userId && x.DeckId == deckId)
            .OrderBy(x => x.Due)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Domain.Entities.Flashcard>> ListDueByDeckAsync(
        Guid userId,
        Guid deckId,
        DateTime before,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(x => x.UserId == userId && x.DeckId == deckId && x.Due <= before)
            .OrderBy(x => x.Due)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Domain.Entities.Flashcard?> GetByIdForUserAsync(
        Guid userId,
        Guid flashcardId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == flashcardId && x.UserId == userId, cancellationToken)
            .ConfigureAwait(false);
    }
}
