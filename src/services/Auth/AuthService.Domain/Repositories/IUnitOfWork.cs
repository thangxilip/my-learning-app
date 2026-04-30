using AuthService.Domain.Entities;
using AuthService.Domain.Repositories.Base;

namespace AuthService.Domain.Repositories;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
