using ExpenseTrackerWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerWebApp.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roles = { "Admin", "User", "Vip" };

            foreach(var rol in roles)
            {
                if(!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }
    }
}
