using System.Text.Json.Serialization;

namespace Flashcard.Application.Contracts;

public record ReviewLogDto(
    byte Rating,
    byte State,
    [property: JsonPropertyName("scheduled_days")] double ScheduledDays,
    [property: JsonPropertyName("review")] DateTime ReviewAt);
