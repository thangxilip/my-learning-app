using Flashcard.Domain.Entities;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Queries;

public record ListDecksQuery(Guid UserId) : IRequest<List<Deck>>;

public class ListDecksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListDecksQuery, List<Deck>>
{
    public async Task<List<Deck>> Handle(ListDecksQuery request, CancellationToken cancellationToken)
    {
        var decks = await unitOfWork.Decks.ListByUserAsync(request.UserId, cancellationToken);
        return decks.ToList();
    }
}
