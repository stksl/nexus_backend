using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostRepository : IPostRepository 
{
    private readonly DbSet<Post> _posts;
    public PostRepository(NexusDbContext dbContext)
    {
        _posts = dbContext.Posts;
    }

    public Task AddPost(Post post)
    {
        return _posts.AddAsync(post).AsTask();
    }

    public void RemovePost(Post post)
    {
        _posts.Remove(post);
    }

    public void UpdatePost(Post updatedPost)
    {
        _posts.Attach(updatedPost).State = EntityState.Modified;
    }
} 