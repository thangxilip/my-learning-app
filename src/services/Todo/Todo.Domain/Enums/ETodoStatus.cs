namespace Todo.Domain.Enums;

public enum ETodoStatus
{
    Active = 0,      // not started / in progress
    Completed = 1,   // done
    Archived = 2     // hidden from main view, not deleted
}