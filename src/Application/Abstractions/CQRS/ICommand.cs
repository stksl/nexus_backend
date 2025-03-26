using MediatR;

namespace Nexus.Application.Abstractions;

public interface ICommand<TResult> : IRequest<Result<TResult>> {}
