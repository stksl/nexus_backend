using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQueryHandler : IRequestHandler<IQuery> {}
public interface IQueryHandler<TResult> : IRequestHandler<IQuery<TResult>, TResult> {}