using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class Comment : IEntity
{
    public int Id {get; init;}
    public int PostId {get; init;}
    public int UserId {get; init;}
    public string Content {get; init;} = null!;
    public DateTime DateCreated {get; init;}
    public DateTime LastModified {get; init;}
}