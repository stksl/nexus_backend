using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record DeletePostCommand(int UserId, int PostId) : ICommand<bool>;