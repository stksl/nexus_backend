using Nexus.Application.Dtos;

namespace Nexus.Application;

public interface ICommentReadRepository 
{
    Task<CommentResponse?> GetCommentWithLikesById(int id);
    Task<IEnumerable<CommentResponse>> GetCommentsWithLikesByPostId(int postId, QueryObject<CommentResponse> queryObject);
}