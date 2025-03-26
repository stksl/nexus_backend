using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult> {}
