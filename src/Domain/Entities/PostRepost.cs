using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostRepost 
{
    public int UserId {get; set;}
    public int PostId {get; set;}
    public DateTime RepostDate {get; set;}
}