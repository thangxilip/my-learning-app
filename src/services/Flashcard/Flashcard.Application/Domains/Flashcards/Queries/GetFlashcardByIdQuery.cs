using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Queries;

public record GetFlashcardByIdQuery(Guid UserId, Guid FlashcardId) : IRequest<FlashcardDto>;

public class GetFlashcardByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFlashcardByIdQuery, FlashcardDto>
{
    public async Task<FlashcardDto> Handle(GetFlashcardByIdQuery request, CancellationToken cancellationToken)
    {
        var flashcard = await unitOfWork.Flashcards.GetByIdForUserAsync(request.UserId, request.FlashcardId, cancellationToken);
        if (flashcard is null)
        {
            throw new NotFoundException("Flashcard not found.");
        }

        return flashcard.ToDto();
    }
}
