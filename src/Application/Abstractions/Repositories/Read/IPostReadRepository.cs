using System.Linq.Expressions;
using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostReadRepository 
{
    Task<Post?> GetPostById(int id);
    Task<IEnumerable<Post>> GetPostsByUser(int userId);
}