using Nexus.Domain.Entities;

namespace Nexus.Application.Dtos;

public class PostResponse : Post
{
    public int LikeCount {get; set;}
    public IEnumerable<string> Tags {get; set;} = null!;
}