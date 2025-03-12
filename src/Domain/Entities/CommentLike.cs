using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class CommentLike : IEntity 
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int CommentId {get; set;}
}