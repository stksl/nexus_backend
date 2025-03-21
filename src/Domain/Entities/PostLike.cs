using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostLike : IEntity 
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
}