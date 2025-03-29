using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class CommentRepository : ICommentRepository
{
    private readonly DbSet<Comment> _comments;
    public CommentRepository(NexusDbContext dbContext)
    {
        _comments = dbContext.Comments;
    }
    public Task AddComment(Comment comment)
    {
        return _comments.AddAsync(comment).AsTask();
    }

    public async Task RemoveComment(int id)
    {
        Comment? comment = await _comments.FindAsync(id);

        if (comment != null)
            _comments.Remove(comment);
    }

    public void UpdateComment(Comment updatedComment)
    {
        _comments.Attach(updatedComment).State = EntityState.Modified;
        
    }

}