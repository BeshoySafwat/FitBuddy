using System.ComponentModel.DataAnnotations;

namespace FitBuddy.API.Helper.DTO.Account
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
