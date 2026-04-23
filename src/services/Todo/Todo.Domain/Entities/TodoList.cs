using BuildingBlocks;

namespace Todo.Domain.Entities;

public class TodoList : AuditableEntity
{
    public Guid? UserId { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; private set; } = "#6B7280";
    public string? Icon { get; private set; }

    public ICollection<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
