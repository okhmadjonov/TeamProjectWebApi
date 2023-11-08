using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TeamProject.AuthToken;
using TeamProject.Data;
using TeamProject.Entity;
using TeamProject.Entity.Enums;
using TeamProject.Entity.LoginViewModel;
using TeamProject.Entity.RegisterViewModel;

namespace TeamProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
     

        public AccountController(UserManager<User> userManager,  SignInManager<User> signInManager)
        {
            _userManager = userManager;
          
            _signInManager = signInManager;
         
        }


        // Login

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null)
            {
                byte[] storedPasswordHashBytes = Convert.FromBase64String(user.PasswordHash);
                string decodedPasswordHash = Encoding.UTF8.GetString(storedPasswordHashBytes);
                bool result = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, decodedPasswordHash);


                if (result)
                {
                    // Password is correct
                    var token = CreateTokenFromUser.CreateToken(user);
                    return Ok(token);
                }
            }

            // Password is incorrect or user not found
            return Unauthorized("Invalid login credentials");
        }

        // Registration

        [HttpPost]
        [Route("Registration")]
        public   async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // User with this email already exists
                return Conflict("User with this email already exists");
            }

            // Create a new user with the provided credentials
            var newUser = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                //PasswordHash = model.Password
            };

            // Hash the password using BCrypt.Net.BCrypt.HashPassword()
            string adminPassword = model.Password;

            // Initialize the inputKey with the adminPassword
            string inputKey = adminPassword;
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(inputKey, salt);
            var base64PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordHash));

            newUser.PasswordHash = base64PasswordHash;


            var newUserResponse = await _userManager.CreateAsync(newUser);
            if (newUserResponse.Succeeded)
            {
                // Add the user to the 'USER' role
                await _userManager.AddToRoleAsync(newUser, (ERole.USER).ToString());

                // Registration successful
                return Ok();
            }

            // Registration failed
            return BadRequest(newUserResponse.Errors);
        }


        // Logout
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }




    }
}
