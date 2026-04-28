using Flashcard.Application.Contracts;
using Flashcard.Domain.Enums;

namespace Flashcard.Application.Common.Mappings;

public static class FlashcardMappings
{
    public static DeckDto ToDto(this Domain.Entities.Deck deck) =>
        new(
            deck.Id,
            deck.Name,
            deck.Description,
            deck.SortOrder,
            deck.CreatedAt,
            deck.UpdatedAt);

    public static FlashcardDto ToDto(this Domain.Entities.Flashcard flashcard) =>
        new(
            flashcard.Id,
            flashcard.DeckId,
            flashcard.Front,
            flashcard.Back,
            flashcard.ToCardDto(),
            Convert.ToBase64String(flashcard.RowVersion),
            flashcard.CreatedAt,
            flashcard.UpdatedAt);

    public static FsrsCardDto ToCardDto(this Domain.Entities.Flashcard flashcard) =>
        new(
            flashcard.Due,
            flashcard.Stability,
            flashcard.Difficulty,
            flashcard.ElapsedDays,
            flashcard.ScheduledDays,
            flashcard.Reps,
            flashcard.Lapses,
            (byte)flashcard.State,
            flashcard.LastReview);

    public static ReviewLogDto ToDto(this Domain.Entities.CardReviewLog log) =>
        new((byte)log.Rating, (byte)log.State, log.ScheduledDays, log.ReviewAt);

    public static FsrsState ToFsrsState(this byte value)
    {
        if (!Enum.IsDefined(typeof(FsrsState), value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "State value is invalid.");
        }

        return (FsrsState)value;
    }

    public static FsrsRating ToFsrsRating(this byte value)
    {
        if (!Enum.IsDefined(typeof(FsrsRating), value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Rating value is invalid.");
        }

        return (FsrsRating)value;
    }
}
