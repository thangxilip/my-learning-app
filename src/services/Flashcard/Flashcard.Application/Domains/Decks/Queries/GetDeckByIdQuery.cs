using Flashcard.Application.Common.Exceptions;
using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Queries;

public record GetDeckByIdQuery(Guid UserId, Guid DeckId) : IRequest<Deck>;

public class GetDeckByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDeckByIdQuery, Deck>
{
    public async Task<Deck> Handle(GetDeckByIdQuery request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        return deck;
    }
}
