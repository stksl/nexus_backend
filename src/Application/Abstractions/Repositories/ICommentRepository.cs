using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface ICommentRepository 
{
    Task AddComment(Comment comment);
    void UpdateComment(Comment updatedComment);
    Task RemoveComment(int id);
}