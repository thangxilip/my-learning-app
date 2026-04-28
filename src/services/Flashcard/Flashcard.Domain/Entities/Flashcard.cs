using BuildingBlocks;
using Flashcard.Domain.Enums;

namespace Flashcard.Domain.Entities;

public class Flashcard : AuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid DeckId { get; private set; }

    public string Front { get; private set; } = string.Empty;
    public string Back { get; private set; } = string.Empty;

    public DateTime Due { get; private set; }
    public double Stability { get; private set; }
    public double Difficulty { get; private set; }
    public double ElapsedDays { get; private set; }
    public double ScheduledDays { get; private set; }
    public int Reps { get; private set; }
    public int Lapses { get; private set; }
    public FsrsState State { get; private set; } = FsrsState.New;
    public DateTime? LastReview { get; private set; }
    public byte[] RowVersion { get; private set; } = Guid.NewGuid().ToByteArray();

    public Deck? Deck { get; private set; }
    public ICollection<CardReviewLog> ReviewLogs { get; private set; } = new List<CardReviewLog>();

    private Flashcard()
    {
    }

    public static Flashcard Create(
        Guid userId,
        Guid deckId,
        string front,
        string back,
        DateTime due,
        double stability,
        double difficulty,
        double elapsedDays,
        double scheduledDays,
        int reps,
        int lapses,
        FsrsState state,
        DateTime? lastReview)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(front);
        ArgumentException.ThrowIfNullOrWhiteSpace(back);
        ValidateMetrics(reps, lapses);

        return new Flashcard
        {
            UserId = userId,
            DeckId = deckId,
            Front = front.Trim(),
            Back = back.Trim(),
            Due = due,
            Stability = stability,
            Difficulty = difficulty,
            ElapsedDays = elapsedDays,
            ScheduledDays = scheduledDays,
            Reps = reps,
            Lapses = lapses,
            State = state,
            LastReview = lastReview
        };
    }

    public void UpdateContent(string front, string back)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(front);
        ArgumentException.ThrowIfNullOrWhiteSpace(back);

        Front = front.Trim();
        Back = back.Trim();
        BumpRowVersion();
        SetUpdated();
    }

    public void UpdateSchedule(
        DateTime due,
        double stability,
        double difficulty,
        double elapsedDays,
        double scheduledDays,
        int reps,
        int lapses,
        FsrsState state,
        DateTime? lastReview)
    {
        ValidateMetrics(reps, lapses);

        Due = due;
        Stability = stability;
        Difficulty = difficulty;
        ElapsedDays = elapsedDays;
        ScheduledDays = scheduledDays;
        Reps = reps;
        Lapses = lapses;
        State = state;
        LastReview = lastReview;
        BumpRowVersion();
        SetUpdated();
    }

    private static void ValidateMetrics(int reps, int lapses)
    {
        if (reps < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reps), "Reps cannot be negative.");
        }

        if (lapses < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lapses), "Lapses cannot be negative.");
        }
    }

    private void BumpRowVersion()
    {
        RowVersion = Guid.NewGuid().ToByteArray();
    }
}
