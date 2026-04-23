using System.Linq.Expressions;
using BuildingBlocks;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Repositories.Base;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repositories.Base;

public class Repository<T>(AppDbContext context) : IRepository<T>
    where T : BaseEntity
{
    protected AppDbContext Context { get; } = context;

    protected DbSet<T> DbSet => Context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyList<T>> ListAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
    }

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().CountAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        await DbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public virtual void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        DbSet.RemoveRange(entities);
    }
}