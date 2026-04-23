using BuildingBlocks;
using Todo.Domain.Enums;

namespace Todo.Domain.Entities;

public class TodoItem : AuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid? ListId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ETodoStatus Status { get; private set; } = ETodoStatus.Active;
    public EPriority Priority { get; private set; } = EPriority.None;

    public DateTime? DueDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public int Position { get; private set; }

    public TodoList? List { get; private set; }
    public ICollection<TodoItemTag> ItemTags { get; private set; } = new List<TodoItemTag>();

    private TodoItem()
    {
    }

    public static TodoItem Create(
        Guid userId,
        Guid? listId,
        string title,
        string? description = null,
        EPriority priority = EPriority.None,
        int position = 0,
        DateTime? dueDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new TodoItem
        {
            UserId = userId,
            ListId = listId,
            Title = title.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Priority = priority,
            Position = position,
            DueDate = dueDate
        };
    }

    public void Rename(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title.Trim();
        SetUpdated();
    }

    public void SetDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        SetUpdated();
    }

    public void MoveToList(Guid? listId)
    {
        ListId = listId;
        SetUpdated();
    }

    public void SetPriority(EPriority priority)
    {
        Priority = priority;
        SetUpdated();
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        SetUpdated();
    }

    public void SetPosition(int position)
    {
        Position = position;
        SetUpdated();
    }

    public void Complete()
    {
        Status = ETodoStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void Reactivate()
    {
        Status = ETodoStatus.Active;
        CompletedAt = null;
        SetUpdated();
    }

    public void Archive()
    {
        Status = ETodoStatus.Archived;
        SetUpdated();
    }
}
