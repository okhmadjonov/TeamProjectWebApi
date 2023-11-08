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

                        var verify = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.PasswordHash);
                        if (verify)
                        {
                            string token = CreateTokenFromUser.CreateToken(user);
                            return Ok(token);
                        }
                        else throw new BadHttpRequestException("Wrong password");

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
        public async Task<IActionResult> Register( RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                
                return Conflict("User with this email already exists");
            }

            var newUser = new User()
            {

                UserName = model.Username,
                Email = model.Email,
                PasswordHash = model.Password

            };
            var userSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var userPasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash, userSalt);

            var newUserResponse = await _userManager.CreateAsync(newUser, userPasswordHash);

            if (newUserResponse.Succeeded)
            
                await _userManager.AddToRoleAsync(newUser, (ERole.USER).ToString());
                return Ok();
            
           
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
