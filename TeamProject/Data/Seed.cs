using Microsoft.AspNetCore.Identity;
using TeamProject.Entity;
using TeamProject.Entity.Enums;
using TeamProject.Entity.LoginViewModel;

namespace TeamProject.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync((ERole.ADMIN).ToString()))
                    await roleManager.CreateAsync(new IdentityRole((ERole.ADMIN).ToString()));
                if (!await roleManager.RoleExistsAsync((ERole.USER).ToString()))
                    await roleManager.CreateAsync(new IdentityRole((ERole.USER).ToString()));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "App-Admin",
                        PasswordHash = "Admin@1234?",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(newAdminUser.PasswordHash, salt);


                    await userManager.CreateAsync(newAdminUser, passwordHash);
                    await userManager.AddToRoleAsync(newAdminUser, (ERole.ADMIN).ToString());
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = "App-User",
                        PasswordHash = "User@1234?",
                        Email = appUserEmail,
                        EmailConfirmed = true,

                    };

                    var userSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    var userPasswordHash = BCrypt.Net.BCrypt.HashPassword(newAppUser.PasswordHash, userSalt);

                    await userManager.CreateAsync(newAppUser, userPasswordHash);
                    await userManager.AddToRoleAsync(newAppUser, (ERole.USER).ToString());
                }
            }
        }
    }
}
