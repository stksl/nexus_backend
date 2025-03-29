using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQuery<TResult> : IRequest<TResult> {}