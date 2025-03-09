using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommandHandler : IRequestHandler<ICommand> {}
public interface ICommandHandler<TResult> : IRequestHandler<ICommand<TResult>, TResult> {}