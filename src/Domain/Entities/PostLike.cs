using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostLike : IEntity 
{
    public int Id {get; init;}
    public int UserId {get; init;}
    public int PostId {get; init;}
}