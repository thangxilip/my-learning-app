using Flashcard.Domain.Repositories;
using Flashcard.Infrastructure.Data;

namespace Flashcard.Infrastructure.Repositories.Base;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FlashcardDbContext _context;

    public UnitOfWork(
        FlashcardDbContext context,
        IDeckRepository decks,
        IFlashcardRepository flashcards,
        ICardReviewLogRepository reviewLogs)
    {
        _context = context;
        Decks = decks;
        Flashcards = flashcards;
        ReviewLogs = reviewLogs;
    }

    public IDeckRepository Decks { get; }
    public IFlashcardRepository Flashcards { get; }
    public ICardReviewLogRepository ReviewLogs { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
