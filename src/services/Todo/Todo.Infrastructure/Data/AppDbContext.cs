using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<TodoItemTag> TodoItemTags => Set<TodoItemTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<TodoItemTag>(builder =>
        {
            builder.HasKey(x => new { x.TodoItemId, x.TagId });

            builder.HasOne(x => x.TodoItem)
                .WithMany(x => x.ItemTags)
                .HasForeignKey(x => x.TodoItemId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.ItemLinks)
                .HasForeignKey(x => x.TagId);
        });
    }
}
