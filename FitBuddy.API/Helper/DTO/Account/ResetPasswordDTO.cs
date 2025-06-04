using System.ComponentModel.DataAnnotations;

namespace FitBuddy.API.Helper.DTO.Account
{
	public class ResetPasswordDTO
	{
		[Required(ErrorMessage = "Password is Required")]
		[RegularExpression("(?=^.{6,15}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_])(?!.*\\s).*$",
			ErrorMessage = "password must have 1 uppercase,1 lowercase,1 number,1 non-alphametric, at least six characters")]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(NewPassword), ErrorMessage = "It should be equial password")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
	}
}
