using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class Comment : IEntity
{
    public int Id {get; set;}
    public int PostId {get; set;}
    public int UserId {get; set;}
    public int? ParentCommentId {get; set;}
    public string Content {get; set;} = null!;
    public DateTime DateCreated {get; set;}
    public DateTime LastModified {get; set;}
}