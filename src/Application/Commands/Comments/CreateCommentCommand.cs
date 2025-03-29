using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record CreateCommentCommand(string Content, int UserId, int PostId, int? ParentCommentId) : ICommand<int>;