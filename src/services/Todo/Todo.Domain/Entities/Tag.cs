using BuildingBlocks;

namespace Todo.Domain.Entities;

public class Tag : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Color { get; private set; } = "#6B7280";

    public ICollection<TodoItemTag> ItemLinks { get; private set; } = new List<TodoItemTag>();

    private Tag()
    {
    }

    public static Tag Create(Guid userId, string name, string? color = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Tag
        {
            UserId = userId,
            Name = name.Trim().ToLowerInvariant(),
            Color = color ?? "#6B7280"
        };
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim().ToLowerInvariant();
        SetUpdated();
    }

    public void SetColor(string hex)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hex);
        Color = hex.Trim();
        SetUpdated();
    }
}
