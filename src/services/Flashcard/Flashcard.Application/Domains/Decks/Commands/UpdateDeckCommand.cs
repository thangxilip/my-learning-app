using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Commands;

public record UpdateDeckCommand(Guid UserId, Guid DeckId, string Name, string? Description, int SortOrder) : IRequest<Deck>;

public class UpdateDeckCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateDeckCommand, Deck>
{
    public async Task<Deck> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        deck.Name = request.Name;
        deck.Description = request.Description;
        unitOfWork.Decks.Update(deck);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return deck;
    }
}
