using BuildingBlocks;
using Flashcard.Domain.Enums;

namespace Flashcard.Domain.Entities;

public class CardReviewLog : AuditableEntity
{
    public Guid FlashcardId { get; private set; }
    public DateTime ReviewAt { get; private set; }
    public FsrsRating Rating { get; private set; }
    public FsrsState State { get; private set; }
    public double ScheduledDays { get; private set; }
    public string? ClientMutationId { get; private set; }

    public Flashcard? Flashcard { get; private set; }

    private CardReviewLog()
    {
    }

    public static CardReviewLog Create(
        Guid flashcardId,
        DateTime reviewAt,
        FsrsRating rating,
        FsrsState state,
        double scheduledDays,
        string? clientMutationId = null)
    {
        return new CardReviewLog
        {
            FlashcardId = flashcardId,
            ReviewAt = reviewAt,
            Rating = rating,
            State = state,
            ScheduledDays = scheduledDays,
            ClientMutationId = string.IsNullOrWhiteSpace(clientMutationId) ? null : clientMutationId.Trim()
        };
    }
}
