using Microsoft.EntityFrameworkCore;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class TagRepository : ITagRepository
{
    private readonly DbSet<Tag> _tags;
    public TagRepository(NexusDbContext dbContext)
    {
        _tags = dbContext.Tags;
    }
    public Task AddTag(Tag tag)
    {
        return _tags.AddAsync(tag).AsTask();
    }
    public Task AddTags(IEnumerable<Tag> tags) 
    {
        return _tags.AddRangeAsync(tags);
    }
    public void RemoveTag(Tag tag)
    {
        _tags.Remove(tag);
    }

    public void UpdateTag(Tag tag)
    {
        _tags.Attach(tag).State = EntityState.Modified;
    }
}