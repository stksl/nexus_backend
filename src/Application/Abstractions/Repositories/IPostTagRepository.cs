using Nexus.Domain.Entities;

namespace Nexus.Application.Abstractions;

public interface IPostTagRepositoryHelper
{
    /// <summary>
    /// Searches for the tag in the corresponding table, otherwise, creates a new one.
    /// Adds a new relation to the conjunction table
    /// </summary>
    /// <param name="tagName"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    Task AttachTags(Post post, IEnumerable<string> tagNames);

}