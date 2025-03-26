using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record CreatePostCommand(string UserId, string Content, string Headline) : ICommand<int>;