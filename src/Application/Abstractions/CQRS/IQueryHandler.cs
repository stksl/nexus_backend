using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result> where TQuery : IQuery {}
