using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand {}
