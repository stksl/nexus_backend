using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>> where TCommand : ICommand<TResult> {}
