using BuildingBlocks;

namespace Flashcard.Domain.Entities;

public class Deck : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Flashcard> Flashcards { get; set; } = [];
}
