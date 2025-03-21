using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommand : IRequest<Result> {}
