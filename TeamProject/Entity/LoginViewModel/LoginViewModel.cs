using System.ComponentModel.DataAnnotations;

namespace TeamProject.Entity.LoginViewModel
{
    public class LoginViewModel
    {


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string Password { get; set; }

    }
}
