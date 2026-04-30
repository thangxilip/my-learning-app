using Flashcard.Application.Common.Exceptions;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Flashcards.Queries;

public record GetFlashcardByIdQuery(Guid UserId, Guid FlashcardId) : IRequest<Domain.Entities.Flashcard>;

public class GetFlashcardByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFlashcardByIdQuery, Domain.Entities.Flashcard>
{
    public async Task<Domain.Entities.Flashcard> Handle(GetFlashcardByIdQuery request, CancellationToken cancellationToken)
    {
        var flashcard = await unitOfWork.Flashcards.GetByIdForUserAsync(request.UserId, request.FlashcardId, cancellationToken);
        if (flashcard is null)
        {
            throw new NotFoundException("Flashcard not found.");
        }

        return flashcard;
    }
}
