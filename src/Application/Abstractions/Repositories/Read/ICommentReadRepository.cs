using Nexus.Domain.Entities;

namespace Nexus.Application;

public interface ICommentReadRepository 
{
    Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
}