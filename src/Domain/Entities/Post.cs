using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public int UserId {get; set;}
    public string Headline {get; set;} = null!;
    public string Content {get; set;} = null!;
    public DateTime DateCreated {get; set;}
    public DateTime LastModified {get; set;}
}