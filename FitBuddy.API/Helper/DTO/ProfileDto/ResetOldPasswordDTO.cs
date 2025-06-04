using System.ComponentModel.DataAnnotations;

namespace FitBuddy.API.Helper.DTO.ProfileDto
{
	public class ResetOldPasswordDTO
	{
		[Required]
		public string OldPassword { get; set; }
		[Required]
		[RegularExpression("(?=^.{6,15}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_])(?!.*\\s).*$",
			ErrorMessage = "password must have 1 uppercase,1 lowercase,1 number,1 non-alphametric, at least six characters")]
		public string NewPassword { get; set; }
	}
}
