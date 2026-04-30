using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using Flashcard.Infrastructure.Data;
using Flashcard.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Flashcard.Infrastructure.Repositories;

public class CardReviewLogRepository(FlashcardDbContext context)
    : Repository<CardReviewLog>(context), ICardReviewLogRepository
{
    public Task<bool> ExistsByClientMutationIdAsync(
        Guid flashcardId,
        string clientMutationId,
        CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking()
            .AnyAsync(
                x => x.FlashcardId == flashcardId && x.ClientMutationId == clientMutationId,
                cancellationToken);
    }
}
