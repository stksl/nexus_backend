using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class CommentLikeRepository : ICommentLikeRepository
{
    private readonly DbSet<CommentLike> _commentLikes;
    public CommentLikeRepository(NexusDbContext dbContext)
    {
        _commentLikes = dbContext.CommentLikes;
    }
    public Task AddCommentLike(int userId, int commentId)
    {
        return _commentLikes.AddAsync(new CommentLike() 
        {
            CommentId = commentId,
            UserId = userId
        }).AsTask();
    }

    public void RemoveCommentLike(int userId, int commentId)
    {
        _commentLikes.Remove(new CommentLike() 
        {
            CommentId = commentId,
            UserId = userId
        });
    }
}