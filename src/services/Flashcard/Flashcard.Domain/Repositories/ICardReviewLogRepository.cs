using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories.Base;

namespace Flashcard.Domain.Repositories;

public interface ICardReviewLogRepository : IRepository<CardReviewLog>
{
    Task<bool> ExistsByClientMutationIdAsync(
        Guid flashcardId,
        string clientMutationId,
        CancellationToken cancellationToken = default);
}
