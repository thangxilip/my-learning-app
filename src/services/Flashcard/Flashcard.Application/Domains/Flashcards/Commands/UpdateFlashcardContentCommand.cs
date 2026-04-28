using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Commands;

public record UpdateFlashcardContentCommand(Guid UserId, Guid FlashcardId, string Front, string Back) : IRequest<FlashcardDto>;

public class UpdateFlashcardContentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateFlashcardContentCommand, FlashcardDto>
{
    public async Task<FlashcardDto> Handle(UpdateFlashcardContentCommand request, CancellationToken cancellationToken)
    {
        var flashcard = await unitOfWork.Flashcards.GetByIdForUserAsync(request.UserId, request.FlashcardId, cancellationToken);
        if (flashcard is null)
        {
            throw new NotFoundException("Flashcard not found.");
        }

        flashcard.UpdateContent(request.Front, request.Back);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return flashcard.ToDto();
    }
}
