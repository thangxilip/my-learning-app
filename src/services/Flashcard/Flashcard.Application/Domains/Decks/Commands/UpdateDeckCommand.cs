using Flashcard.Application.Common.Exceptions;
using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Commands;

public record UpdateDeckCommand(Guid UserId, Guid DeckId, string Name, string? Description, int SortOrder) : IRequest<DeckDto>;

public class UpdateDeckCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateDeckCommand, DeckDto>
{
    public async Task<DeckDto> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.Decks.GetByIdForUserAsync(request.UserId, request.DeckId, cancellationToken);
        if (deck is null)
        {
            throw new NotFoundException("Deck not found.");
        }

        deck.Update(request.Name, request.Description, request.SortOrder);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return deck.ToDto();
    }
}
