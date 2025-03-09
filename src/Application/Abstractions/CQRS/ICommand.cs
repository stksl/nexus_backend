using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommand : IRequest {}
public interface ICommand<TResult> : IRequest<TResult> {}