using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostRepository 
{
    Task AddPost(Post post);
    void UpdatePost(Post updatedPost);
    Task RemovePost(int id);
}