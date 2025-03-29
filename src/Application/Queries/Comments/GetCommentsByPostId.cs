using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetCommentsByPostId(int PostId, int PageNumber) : IQuery<IEnumerable<Comment>>;