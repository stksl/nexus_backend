using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostReadRepository 
{
    Task<Post?> GetPostById(int id);
}