using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetPostsByUserQuery(int UserId, int PageNumber) : IQuery<IEnumerable<Post>>;