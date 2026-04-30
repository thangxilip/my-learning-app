namespace Flashcard.Application.Contracts;

public record DeckDto(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder,
    DateTime CreatedAt,
    DateTime UpdatedAt);
