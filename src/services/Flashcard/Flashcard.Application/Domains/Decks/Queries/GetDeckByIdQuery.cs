using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Queries;

public record GetDeckByIdQuery(Guid UserId, Guid DeckId) : IRequest<DeckDto>;

public class GetDeckByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDeckByIdQuery, DeckDto>
{
    public async Task<DeckDto> Handle(GetDeckByIdQuery request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        return deck.ToDto();
    }
}
