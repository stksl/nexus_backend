using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record UpdatePostCommand(int PostId, int UserId, string Content, string Headline, IEnumerable<string> Tags) : ICommand<bool>;