using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class CommentLike 
{
    public int UserId {get; set;}
    public int CommentId {get; set;}
}