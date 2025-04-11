using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostTagRepository : IPostTagRepository
{
    private readonly DbSet<PostTag> _postTags;
    private readonly ITagReadRepository _tagReadRepository;
    public PostTagRepository(NexusDbContext dbContext, ITagReadRepository tagReadRepository)
    {
        _postTags = dbContext.PostTags;
        _tagReadRepository = tagReadRepository;
    }
    public async Task AttachTags(Post post, IEnumerable<string> tagNames)
    {
        foreach(string tagName in tagNames) 
        {
            Tag? tag = await _tagReadRepository.GetTagByName(tagName);
            if (tag != null)
                await _postTags.AddAsync(new PostTag() {PostId = post.Id, TagId = tag.Id});
        }
    }
    public void ClearTags(Post post) 
    {
        _postTags.RemoveRange(_postTags.Where(pt => pt.PostId == post.Id));
    }
}