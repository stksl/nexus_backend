using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record DeleteCommentCommand(int UserId, int CommentId) : ICommand<bool>;