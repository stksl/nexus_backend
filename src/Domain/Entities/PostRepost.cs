using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostRepost : IEntity 
{
    public int Id {get; init;}
    public int UserId {get; init;}
    public int PostId {get; init;}
    public DateTime RepostDate {get; init;}
}