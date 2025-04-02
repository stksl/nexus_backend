using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostTagRepositoryHelper : IPostTagRepositoryHelper
{
    private readonly DbSet<PostTag> _postTags;
    private readonly ITagReadRepository _tagReadRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PostTagRepositoryHelper(NexusDbContext dbContext,
        ITagReadRepository tagReadRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _postTags = dbContext.PostTags;
        _tagReadRepository = tagReadRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task AttachTags(Post post, IEnumerable<string> tagNames)
    {
        List<Tag> tags = new List<Tag>();
        foreach (string tagName in tagNames)
        {
            if (tags.FirstOrDefault(t => t.Name == tagName) != null)
                continue;

            Tag? tag = await _tagReadRepository.GetTagByName(tagName);
            if (tag == null)
            {
                tag = new Tag()
                {
                    Name = tagName
                };
                await _tagRepository.AddTag(tag);
            }

            tags.Add(tag);
        }

        await _unitOfWork.SaveChangesAsync();

        foreach (Tag tag in tags)
        {
            PostTag? existing = await _postTags.FindAsync(post.Id, tag.Id);
            if (existing == null)
            {
                await _postTags.AddAsync(new()
                {
                    PostId = post.Id,
                    TagId = tag.Id
                });
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}