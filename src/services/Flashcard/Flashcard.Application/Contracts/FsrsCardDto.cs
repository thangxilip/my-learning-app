using System.Text.Json.Serialization;

namespace Flashcard.Application.Contracts;

public record FsrsCardDto(
    DateTime Due,
    double Stability,
    double Difficulty,
    [property: JsonPropertyName("elapsed_days")] double ElapsedDays,
    [property: JsonPropertyName("scheduled_days")] double ScheduledDays,
    int Reps,
    int Lapses,
    byte State,
    [property: JsonPropertyName("last_review")] DateTime? LastReview);
