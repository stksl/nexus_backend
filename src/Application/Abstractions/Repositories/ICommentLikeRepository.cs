using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface ICommentLikeRepository 
{
    Task AddCommentLike(int userId, int commentId);
    void RemoveCommentLike(int userId, int commentId);
}