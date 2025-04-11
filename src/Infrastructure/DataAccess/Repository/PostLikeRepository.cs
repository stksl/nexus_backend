using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostLikeRepository : IPostLikeRepository
{
    private readonly DbSet<PostLike> _postLikes;
    public PostLikeRepository(NexusDbContext dbContext)
    {
        _postLikes = dbContext.PostLikes;
    }
    public Task AddPostLike(int userId, int postId)
    {
        return _postLikes.AddAsync(new PostLike() 
        {
            PostId = postId,
            UserId = userId
        }).AsTask();
    }

    public void RemovePostLike(int userId, int postId)
    {
        _postLikes.Remove(new PostLike() 
        {
            PostId = postId,
            UserId = userId
        });
    }
}