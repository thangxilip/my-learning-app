namespace Todo.Domain.Entities;

public class TodoItemTag
{
    public Guid TodoItemId { get; private set; }
    public Guid TagId      { get; private set; }

    public TodoItem? TodoItem { get; private set; }
    public Tag?      Tag      { get; private set; }

    private TodoItemTag() { }

    internal static TodoItemTag Create(Guid todoItemId, Guid tagId) =>
        new() { TodoItemId = todoItemId, TagId = tagId };
}