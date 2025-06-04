using System.ComponentModel.DataAnnotations;

namespace FitBuddy.API.Helper.DTO.Account
{
    public class ForgetPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Email not valid")]
        [Required]
        public string Email { get; set; }
    }
}
