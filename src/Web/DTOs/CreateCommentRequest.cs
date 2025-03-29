namespace Nexus.WebApi;

public record CreateCommentRequest(int PostId, string Content, int? ParentCommentId);