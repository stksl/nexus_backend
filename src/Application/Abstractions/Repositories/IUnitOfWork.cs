namespace Nexus.Application.Abstractions;

public interface IUnitOfWork 
{
    Task<int> SaveChangesAsync();
}