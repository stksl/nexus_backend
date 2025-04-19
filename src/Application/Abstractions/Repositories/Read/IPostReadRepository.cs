using Nexus.Application.Dtos;

namespace Nexus.Application.Abstractions;

public interface IPostReadRepository 
{
    Task<PostResponse?> GetPostWithLikesById(int id);
    Task<IEnumerable<PostResponse>> GetPostsWithLikesByUser(int userId, QueryObject<PostResponse> queryObject);
}