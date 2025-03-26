using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetPostsByUserQuery(int userId) : IQuery<IEnumerable<Post>>;