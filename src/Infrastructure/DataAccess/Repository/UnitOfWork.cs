using Nexus.Application.Abstractions;

namespace Nexus.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly NexusDbContext _dbContext;
    public UnitOfWork(NexusDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}