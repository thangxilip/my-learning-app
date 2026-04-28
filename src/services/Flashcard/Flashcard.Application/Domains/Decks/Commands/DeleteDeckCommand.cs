using Flashcard.Application.Common.Exceptions;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Commands;

public record DeleteDeckCommand(Guid UserId, Guid DeckId) : IRequest;

public class DeleteDeckCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDeckCommand>
{
    public async Task Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        deck.SoftDelete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
