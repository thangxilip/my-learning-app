namespace Flashcard.API.Contracts;

public record CreateDeckRequest(string Name, string? Description, int SortOrder = 0);

public record UpdateDeckRequest(string Name, string? Description, int SortOrder = 0);
