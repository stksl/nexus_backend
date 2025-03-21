using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record GetPostByIdQuery(int Id) : IQuery;