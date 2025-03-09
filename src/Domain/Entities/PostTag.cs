using Nexus.SharedKernel;

namespace Nexus.Domain.Entities;

public class PostTag : IEntity 
{
    public int Id {get; init;}
    public string Name {get; init;} = null!;
}