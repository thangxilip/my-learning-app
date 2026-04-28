using Flashcard.Application.Common.Mappings;
using Flashcard.Application.Contracts;
using Flashcard.Domain.Repositories;
using MediatR;

namespace Flashcard.Application.Domains.Decks.Queries;

public record ListDecksQuery(Guid UserId) : IRequest<List<DeckDto>>;

public class ListDecksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListDecksQuery, List<DeckDto>>
{
    public async Task<List<DeckDto>> Handle(ListDecksQuery request, CancellationToken cancellationToken)
    {
        var decks = await unitOfWork.Decks.ListByUserAsync(request.UserId, cancellationToken);
        return decks.Select(x => x.ToDto()).ToList();
    }
}
