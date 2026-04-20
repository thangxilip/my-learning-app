using BuildingBlocks;

namespace Todo.Domain.Entities;

public class TodoList : AuditableEntity
{
    public Guid   UserId { get; private set; }
    public string Name   { get; private set; } = string.Empty;
    public string Color  { get; private set; } = "#6B7280";
    public string? Icon  { get; private set; }  // e.g. "📚"
    public int    Position { get; private set; }
    
    public IReadOnlyCollection<TodoItem> Items { get; set; } = new List<TodoItem>();
}