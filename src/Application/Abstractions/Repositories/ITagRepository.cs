using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface ITagRepository 
{
    Task AddTag(Tag tag);
    void UpdateTag(Tag tag);
    void RemoveTag(Tag tag);
}