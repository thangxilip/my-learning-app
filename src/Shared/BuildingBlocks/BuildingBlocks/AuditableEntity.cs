namespace BuildingBlocks;

public class AuditableEntity : BaseEntity
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        SetUpdated();
    }
}