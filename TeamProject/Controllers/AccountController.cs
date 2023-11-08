using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
      
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) { 
        
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
           
            if (user != null) {

                // User is found, check password

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck) {

                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password,  false, false);
                    if (result.Succeeded) {

                     
                            string token = CreateTokenFromUser.CreateToken(user);
                            return Ok(token);
                      
                    }

                }

  
                // Password is Incorrrect
                return Unauthorized("Password is Incorrect");
            }
            // User not found
            return Unauthorized("User not found");

        }


        // Registration

        [HttpPost]
        [Route("Registration")]
        public  async Task<IActionResult> Register(RegisterViewModel model)
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
                PasswordHash = model.Password
            };

            // Generate a salt and hash the password

            var userSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var userPasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash, userSalt);

            // Create the user in the database

            var newUserResponse = await _userManager.CreateAsync(newUser, userPasswordHash);
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
