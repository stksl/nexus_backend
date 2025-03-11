using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record CreatePostCommand(int UserId, string Content, string Headline) : ICommand;