using Nexus.SharedKernel;

namespace Nexus.Application.Auth;

public class RefreshToken : IEntity
{
    public int Id {get; set;}
    public string Value {get; set;} = null!;
    public int UserId {get; set;}
    public DateTime Expires {get; set;}
}