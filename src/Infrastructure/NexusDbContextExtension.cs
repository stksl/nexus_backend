using Microsoft.AspNetCore.Identity;

namespace Nexus.Infrastructure.Extensions;

public static class NexusDbContextExtension 
{
    public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager) 
    {
        if (await roleManager.RoleExistsAsync(AppRoles.User))
            return;
            
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