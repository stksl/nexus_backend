using Microsoft.AspNetCore.Identity;

namespace Nexus.Infrastructure.DataAccess;

public class AppUser : IdentityUser<int>
{
    public AppUser()
    {
    }
}