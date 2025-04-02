using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface ITagReadRepository 
{
    Task<Tag?> GetTagByName(string name);

}