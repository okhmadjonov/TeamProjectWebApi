using System.ComponentModel.DataAnnotations;

namespace TeamProject.Entity.RegisterViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string Password { get; set; }
    }
}
