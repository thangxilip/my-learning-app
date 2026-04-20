using BuildingBlocks;
using Todo.Domain.Enums;

namespace Todo.Domain.Entities;

public class TodoItem : AuditableEntity
{
    // ── Identity ─────────────────────────────────
    public Guid   UserId  { get; private set; }
    public Guid?  ListId  { get; private set; }  // optional grouping

    // ── Core fields ───────────────────────────────
    public string   Title       { get; private set; } = string.Empty;
    public string?  Description { get; private set; }
    public ETodoStatus Status   { get; private set; } = ETodoStatus.Active;
    public EPriority   Priority  { get; private set; } = EPriority.None;

    // ── Scheduling ────────────────────────────────
    public DateTime? DueDate     { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // ── Ordering ──────────────────────────────────
    public int Position { get; private set; }  // for drag-to-reorder

    // ── Navigation ────────────────────────────────
    public TodoList? List { get; set; }
    public List<TodoItemTag> Tags { get; set; } = [];
}