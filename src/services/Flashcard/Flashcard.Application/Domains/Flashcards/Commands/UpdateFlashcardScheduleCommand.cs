using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Commands;

public record UpdateFlashcardScheduleCommand(
    Guid UserId,
    Guid FlashcardId,
    FsrsCardDto Card,
    string? ExpectedRowVersion) : IRequest<FlashcardDto>;

public class UpdateFlashcardScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateFlashcardScheduleCommand, FlashcardDto>
{
    public async Task<FlashcardDto> Handle(UpdateFlashcardScheduleCommand request, CancellationToken cancellationToken)
    {
        // var flashcard = await unitOfWork.Flashcards.GetByIdForUserAsync(request.UserId, request.FlashcardId, cancellationToken);
        // if (flashcard is null)
        // {
        //     throw new NotFoundException("Flashcard not found.");
        // }
        //
        // ValidateRowVersion(request.ExpectedRowVersion, flashcard.RowVersion);
        //
        // var card = request.Card;
        // flashcard.UpdateSchedule(
        //     card.Due,
        //     card.Stability,
        //     card.Difficulty,
        //     card.ElapsedDays,
        //     card.ScheduledDays,
        //     card.Reps,
        //     card.Lapses,
        //     card.State.ToFsrsState(),
        //     card.LastReview);
        //
        // await unitOfWork.SaveChangesAsync(cancellationToken);
        // return flashcard.ToDto();
        throw new NotImplementedException();
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
