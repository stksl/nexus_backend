using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQuery : IRequest<Result> {}