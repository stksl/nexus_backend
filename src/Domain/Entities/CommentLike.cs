using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class CommentLike : IEntity 
{
    public int Id {get; init;}
    public int UserId {get; init;}
    public int CommentId {get; init;}
}