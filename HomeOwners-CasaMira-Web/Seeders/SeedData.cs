using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HomeOwners_CasaMira_Web.Models;
using System;
using System.Threading.Tasks;

namespace HomeOwners_CasaMira_Web.Seeders
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if roles exist, if not create them
            var roles = new[] { "Admin", "Homeowner", "Staff" };
            
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check if the Admin user exists
            var adminEmail = "admin@casamira.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new Users
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User", // Custom Full Name
                    Address = "123 Admin St", // Custom Address
                    DateOfBirth = new DateTime(1990, 1, 1), // Custom Date of Birth
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

                if (result.Succeeded)
                {
                    // Assign the Admin role to the user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
