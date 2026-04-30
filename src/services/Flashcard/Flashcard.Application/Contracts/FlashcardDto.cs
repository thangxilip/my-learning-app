namespace Flashcard.Application.Contracts;

public record FlashcardDto(
    Guid Id,
    Guid DeckId,
    string Front,
    string Back,
    FsrsCardDto Card,
    string RowVersion,
    DateTime CreatedAt,
    DateTime UpdatedAt);
