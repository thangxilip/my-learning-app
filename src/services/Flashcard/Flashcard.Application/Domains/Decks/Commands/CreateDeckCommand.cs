using Flashcard.Application.Contracts;
using Flashcard.Application.Common.Mappings;
using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Commands;

public record CreateDeckCommand(Guid UserId, string Name, string? Description, int SortOrder) : IRequest<DeckDto>;

public class CreateDeckCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateDeckCommand, DeckDto>
{
    public async Task<DeckDto> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = Deck.Create(request.UserId, request.Name, request.Description, request.SortOrder);
        await unitOfWork.Decks.AddAsync(deck, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return deck.ToDto();
    }
}
