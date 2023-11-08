using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Data;
using TeamProject.Entity;
using TeamProject.Entity.LoginViewModel;
using TeamProject.Entity.RegisterViewModel;

namespace TeamProject.Controllers
{
 
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;


        public AccountController(UserManager<User> userManager, AppDbContext appDbContext, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _context = appDbContext;
            _signInManager = signInManager;

        }


        // Login

        [HttpPost]
      
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) { 
        
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
           
            if (user != null) {

                // User is found, check password

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck) {

                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password,  false, false);
                    if (result.Succeeded) {

                       
                        return Ok();
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
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
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

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
                return Ok();
            }
            return BadRequest("Registration Failed");
        }



        // Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }




    }
}
