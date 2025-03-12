using System.Linq.Expressions;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record GetPostsQuery(Expression<Func<Post, bool>> Expression) : IQuery;