using Microsoft.AspNetCore.Identity;

namespace Nexus.Infrastructure;

public static class NexusDbContextExtension 
{
    public static void SeedRoles(RoleManager<IdentityRole<int>> roleManager) 
    {
        roleManager.CreateAsync(new IdentityRole<int>() 
        {
            Id = 1,
            Name = AppRoles.User,
            NormalizedName = AppRoles.User.ToUpper()
        }).Wait();
        roleManager.CreateAsync(new IdentityRole<int>() 
        {
            Id = 2,
            Name = AppRoles.Admin,
            NormalizedName = AppRoles.Admin.ToUpper()
        }).Wait();
    }
}