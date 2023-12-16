using Kursova.Constants;
using Microsoft.AspNetCore.Identity;

namespace Kursova.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider provider)
        {
            var userManager = provider.GetService<UserManager<IdentityUser>>();
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();
            //Add Roles To Db

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Create admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var isUserExists = await userManager.FindByEmailAsync(admin.Email);

            if (isUserExists == null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }


        }
    }
}
