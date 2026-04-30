using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.Repositories.Base;
using AuthService.Infrastructure.Data;

namespace AuthService.Infrastructure.Repositories.Base;

public sealed class UnitOfWork(AuthDbContext context) : IUnitOfWork
{
    private IRepository<User> _userRepository;

    public IRepository<User> Users => _userRepository ??= new Repository<User>(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
