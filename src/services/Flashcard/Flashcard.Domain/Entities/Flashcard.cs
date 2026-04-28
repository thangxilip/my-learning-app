using BuildingBlocks;
using Flashcard.Domain.Enums;

namespace Flashcard.Domain.Entities;

public class Flashcard : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid DeckId { get; set; }

    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;

    public DateTime Due { get; set; }
    public double Stability { get; set; }
    public double Difficulty { get; set; }
    public double ElapsedDays { get; set; }
    public double ScheduledDays { get; set; }
    public int Reps { get; set; }
    public int Lapses { get; set; }
    public FsrsState State { get; set; } = FsrsState.New;
    public DateTime? LastReview { get; set; }
    public byte[] RowVersion { get; set; } = Guid.NewGuid().ToByteArray();

    public Deck? Deck { get; set; }
    public ICollection<CardReviewLog> ReviewLogs { get; private set; } = new List<CardReviewLog>();

    public Flashcard()
    {
    }

    public void Touch() => SetUpdated();
}
