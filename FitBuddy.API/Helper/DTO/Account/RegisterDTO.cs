using FitBuddy.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FitBuddy.API.Helper.DTO.Account
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression("(?=^.{6,15}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_])(?!.*\\s).*$",
            ErrorMessage = "password must have 1 uppercase,1 lowercase,1 number,1 non-alphametric, at least six characters")]
        public string Password { get; set; } = null!;

        [Range(10,100,ErrorMessage ="The Age Must Be From 0 : 100") ]
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "The Tall Must Be Greater than Zero. ")]
        public decimal? Tall { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "The Weight Must Be Greater than Zero. ")]
        public decimal? Weight { get; set; }
    }
}
