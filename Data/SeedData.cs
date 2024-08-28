using Microsoft.AspNetCore.Identity;

namespace NetflixSharp.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com"
                };

                var createPowerUser = await userManager.CreateAsync(user, "AdminPassword123!");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}