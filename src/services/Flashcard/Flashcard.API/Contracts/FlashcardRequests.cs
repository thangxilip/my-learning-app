using Flashcard.Application.Contracts;

namespace Flashcard.API.Contracts;

public record CreateFlashcardRequest(string Front, string Back, FsrsCardDto Card);

public record UpdateFlashcardContentRequest(string Front, string Back);

public record UpdateFlashcardScheduleRequest(FsrsCardDto Card, string? ExpectedRowVersion);

public record SubmitFlashcardReviewRequest(
    FsrsCardDto Card,
    ReviewLogDto Log,
    string? ExpectedRowVersion,
    string? ClientMutationId);
