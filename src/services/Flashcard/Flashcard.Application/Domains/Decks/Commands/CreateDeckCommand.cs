using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Commands;

public record CreateDeckCommand(Guid UserId, string Name, string? Description, int SortOrder) : IRequest<Deck>;

public class CreateDeckCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateDeckCommand, Deck>
{
    public async Task<Deck> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = new Deck
        {
            UserId = request.UserId,
            Name = request.Name,
            Description = request.Description,
        };
        await unitOfWork.Decks.AddAsync(deck, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return deck;
    }
}
