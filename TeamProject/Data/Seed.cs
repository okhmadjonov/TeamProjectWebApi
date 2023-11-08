using Microsoft.AspNetCore.Identity;
using System.Text;
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
                {
                    await roleManager.CreateAsync(new IdentityRole((ERole.ADMIN).ToString()));
                }

                if (!await roleManager.RoleExistsAsync((ERole.USER).ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole((ERole.USER).ToString()));
                }

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "App-Admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };

                    // Hash the password using BCrypt.Net.BCrypt.HashPassword()
                    string adminPassword = "Admin@1234?";

                    // Initialize the inputKey with the adminPassword
                    string inputKey = adminPassword;
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(inputKey, salt);
                    var base64PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordHash));

                    newAdminUser.PasswordHash = base64PasswordHash;


                    await userManager.CreateAsync(newAdminUser);
                    await userManager.AddToRoleAsync(newAdminUser, (ERole.ADMIN).ToString());
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);

                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = "App-User",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };

                    // Hash the password using BCrypt.Net.BCrypt.HashPassword()
                    string userPassword = "User@1234?";

                    // Initialize the inputKey with the userPassword
                    string inputKey = userPassword;
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(inputKey, salt);
                    var base64PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordHash));

                    newAppUser.PasswordHash = base64PasswordHash;

                    await userManager.CreateAsync(newAppUser);
                    await userManager.AddToRoleAsync(newAppUser, (ERole.USER).ToString());
                }
            }
        }
    }
}
