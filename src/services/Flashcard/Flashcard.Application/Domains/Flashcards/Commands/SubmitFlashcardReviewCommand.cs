using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Entities;
using Flashcard.Domain.Enums;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Commands;

public record SubmitFlashcardReviewCommand(
    Guid UserId,
    Guid FlashcardId,
    FsrsCardDto Card,
    ReviewLogDto Log,
    string? ExpectedRowVersion,
    string? ClientMutationId) : IRequest<ReviewSyncResultDto>;

public class SubmitFlashcardReviewCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitFlashcardReviewCommand, ReviewSyncResultDto>
{
    public async Task<ReviewSyncResultDto> Handle(SubmitFlashcardReviewCommand request, CancellationToken cancellationToken)
    {
        var flashcard = await unitOfWork.Flashcards.GetByIdForUserAsync(request.UserId, request.FlashcardId, cancellationToken);
        if (flashcard is null)
        {
            throw new NotFoundException("Flashcard not found.");
        }

        ValidateRowVersion(request.ExpectedRowVersion, flashcard.RowVersion);

        if (!string.IsNullOrWhiteSpace(request.ClientMutationId))
        {
            var existed = await unitOfWork.ReviewLogs.ExistsByClientMutationIdAsync(
                request.FlashcardId,
                request.ClientMutationId,
                cancellationToken);

            if (existed)
            {
                throw new ConflictException("This review mutation was already applied.");
            }
        }

        var card = request.Card;
        flashcard.Due = card.Due;
        flashcard.Stability = card.Stability;
        flashcard.Difficulty = card.Difficulty;
        flashcard.ElapsedDays = card.ElapsedDays;
        flashcard.ScheduledDays = card.ScheduledDays;
        flashcard.Reps = card.Reps;
        flashcard.Lapses = card.Lapses;
        flashcard.State = ToFsrsState(card.State);
        flashcard.LastReview = card.LastReview;
        flashcard.RowVersion = Guid.NewGuid().ToByteArray();
        flashcard.Touch();

        var reviewLog = new CardReviewLog
        {
            FlashcardId = request.FlashcardId,
            ReviewAt = request.Log.ReviewAt,
            Rating = ToFsrsRating(request.Log.Rating),
            State = ToFsrsState(request.Log.State),
            ScheduledDays = request.Log.ScheduledDays,
            ClientMutationId = string.IsNullOrWhiteSpace(request.ClientMutationId)
                ? null
                : request.ClientMutationId.Trim()
        };

        await unitOfWork.ReviewLogs.AddAsync(reviewLog, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewSyncResultDto(ToFlashcardDto(flashcard), ToReviewLogDto(reviewLog));
    }

    private static void ValidateRowVersion(string? expectedRowVersion, byte[] actualRowVersion)
    {
        if (string.IsNullOrWhiteSpace(expectedRowVersion))
        {
            return;
        }

        byte[] expectedBytes;
        try
        {
            expectedBytes = Convert.FromBase64String(expectedRowVersion);
        }
        catch (FormatException)
        {
            throw new ConflictException("Expected row version is not valid base64.");
        }

        if (!actualRowVersion.AsSpan().SequenceEqual(expectedBytes))
        {
            throw new ConflictException("Flashcard was changed by another operation.");
        }
    }

    private static FsrsState ToFsrsState(byte value)
    {
        if (!Enum.IsDefined(typeof(FsrsState), value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "State value is invalid.");
        }

        return (FsrsState)value;
    }

    private static FsrsRating ToFsrsRating(byte value)
    {
        if (!Enum.IsDefined(typeof(FsrsRating), value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Rating value is invalid.");
        }

        return (FsrsRating)value;
    }

    private static FlashcardDto ToFlashcardDto(Domain.Entities.Flashcard f)
    {
        var card = new FsrsCardDto(
            f.Due,
            f.Stability,
            f.Difficulty,
            f.ElapsedDays,
            f.ScheduledDays,
            f.Reps,
            f.Lapses,
            (byte)f.State,
            f.LastReview);

        return new FlashcardDto(
            f.Id,
            f.DeckId,
            f.Front,
            f.Back,
            card,
            Convert.ToBase64String(f.RowVersion),
            f.CreatedAt,
            f.UpdatedAt);
    }

    private static ReviewLogDto ToReviewLogDto(CardReviewLog log) =>
        new((byte)log.Rating, (byte)log.State, log.ScheduledDays, log.ReviewAt);
}
