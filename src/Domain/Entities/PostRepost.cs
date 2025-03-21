using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostRepost : IEntity 
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
    public DateTime RepostDate {get; set;}
}