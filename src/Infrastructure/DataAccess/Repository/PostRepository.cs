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

    public Task AddPosts(IEnumerable<Post> posts)
    {
        return _posts.AddRangeAsync(posts);
    }

    public async Task RemovePost(int id)
    {
        Post? existing = await _posts.FindAsync(id);

        if (existing != null)
            _posts.Remove(existing);
    }

    public async Task<bool> UpdatePost(Post updatedPost)
    {
        Post? existing = await _posts.FindAsync(updatedPost.Id);

        if (existing == null)
            return false;
        
        _posts.Update(existing);
        return true;
    }
} 