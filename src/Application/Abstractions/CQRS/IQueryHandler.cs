using MediatR;

namespace Nexus.Application.Abstractions;

public interface IQueryHandler : IRequestHandler<IQuery> {}
