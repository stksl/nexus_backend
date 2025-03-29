using Nexus.Domain.Entities;

namespace Nexus.Application;

public interface ICommentReadRepository 
{
    Task<Comment?> GetCommentById(int id);
    Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, QueryObject queryObject);
}