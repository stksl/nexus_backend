using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetCommentsByPostId(int PostId) : IQuery<IEnumerable<Comment>>;