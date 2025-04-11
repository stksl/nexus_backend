using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostLikeRepository 
{
    Task AddPostLike(int userId, int postId);
    void RemovePostLike(int userId, int postId);
}