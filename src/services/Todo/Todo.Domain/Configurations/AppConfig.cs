namespace Todo.Domain.Configurations;


public record AppConfig
{
    public required ConnectionStrings ConnectionStrings { get; set; }
}

public record ConnectionStrings
{
    public string DefaultConnection { get; set; }
}