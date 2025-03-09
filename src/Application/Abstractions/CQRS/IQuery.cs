using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQuery : IRequest {}
public interface IQuery<TResult> : IRequest<TResult> {}