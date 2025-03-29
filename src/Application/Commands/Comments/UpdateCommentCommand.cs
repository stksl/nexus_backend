using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record UpdateCommentCommand(int UserId, int CommentId, string Content) : ICommand<bool>;