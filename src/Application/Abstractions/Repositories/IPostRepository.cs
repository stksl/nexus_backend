using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostRepository 
{
    Task AddPosts(IEnumerable<Post> posts);
    Task AddPost(Post post);

    Task<bool> UpdatePost(Post updatedPost);
    Task RemovePost(int id);
}