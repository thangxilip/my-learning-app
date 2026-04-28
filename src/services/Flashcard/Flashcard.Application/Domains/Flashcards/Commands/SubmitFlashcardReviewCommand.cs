using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
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

        flashcard.UpdateSchedule(
            request.Card.Due,
            request.Card.Stability,
            request.Card.Difficulty,
            request.Card.ElapsedDays,
            request.Card.ScheduledDays,
            request.Card.Reps,
            request.Card.Lapses,
            request.Card.State.ToFsrsState(),
            request.Card.LastReview);

        var reviewLog = Domain.Entities.CardReviewLog.Create(
            request.FlashcardId,
            request.Log.ReviewAt,
            request.Log.Rating.ToFsrsRating(),
            request.Log.State.ToFsrsState(),
            request.Log.ScheduledDays,
            request.ClientMutationId);

        await unitOfWork.ReviewLogs.AddAsync(reviewLog, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewSyncResultDto(flashcard.ToDto(), reviewLog.ToDto());
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
}
