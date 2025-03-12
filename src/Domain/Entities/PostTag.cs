using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostTag : IEntity 
{
    public int Id {get; set;}
    public string Name {get; set;} = null!;
}