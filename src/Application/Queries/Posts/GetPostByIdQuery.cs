using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetPostByIdQuery(int Id) : IQuery<Post?>;