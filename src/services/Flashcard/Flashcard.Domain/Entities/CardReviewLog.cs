using BuildingBlocks;
using Flashcard.Domain.Enums;

namespace Flashcard.Domain.Entities;

public class CardReviewLog : AuditableEntity
{
    public Guid FlashcardId { get; set; }
    public DateTime ReviewAt { get; set; }
    public FsrsRating Rating { get; set; }
    public FsrsState State { get; set; }
    public double ScheduledDays { get; set; }
    public string? ClientMutationId { get; set; }

    public Flashcard? Flashcard { get; set; }
}
