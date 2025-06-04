using ECommerce.API.Helper.Error;
using FitBuddy.API.Helper.DTO.ProfileDto;
using FitBuddy.Core.Entities;
using FitBuddy.Core.Repositroy.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FitBuddy.API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IGenericRepositroy<ApplicationUser> _profile;
        private readonly UserManager<ApplicationUser> _user;
		private readonly SignInManager<ApplicationUser> _signIn;
		private readonly ICloudinaryService _cloudinary;

		public ProfileController(IGenericRepositroy<ApplicationUser> profile
            , UserManager<ApplicationUser> user,SignInManager<ApplicationUser> signIn,
			ICloudinaryService cloudinary)
        {
            _profile = profile;
            _user = user;
			_signIn = signIn;
			_cloudinary = cloudinary;
		}
        [HttpGet]
        public async Task<ActionResult<UserData>> GetUser()
        {
            ApplicationUser? user = await GETUSERBYTOKEN();
            UserData profile = user;
            return Ok(profile);
        }


        [HttpPost("age/{age:int}")]
        public async Task<ActionResult<UserData>> EditAge(int age)
        {
            ApplicationUser? user = await GETUSERBYTOKEN();
            user.Age = age;
            var result = await _user.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
            UserData profile = user;
            return Ok(profile);
        }
        [HttpPost("weight/{weight:decimal}")]
        public async Task<ActionResult<UserData>> EditWeight(decimal weight)
        {
            ApplicationUser? user = await GETUSERBYTOKEN();
            user.Weight = weight;
            var result = await _user.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
            UserData profile = user;
            return Ok(profile);
        }
        [HttpPost("tall/{tall:decimal}")]
        public async Task<ActionResult<UserData>> EditTall(decimal tall)
        {
            ApplicationUser? user = await GETUSERBYTOKEN();
            user.Tall = tall;
            var result = await _user.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
            UserData profile = user;
            return Ok(profile);
        }
        [HttpPost]
        public async Task<ActionResult<UserData>> Edit(string displayName)
        {
            ApplicationUser? user = await GETUSERBYTOKEN();
            user.DisplayName = displayName;
            var result = await _user.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
            UserData profile = user;
            return Ok(profile);
        }
        [HttpPost("EditImage")]
        public async Task<ActionResult<UserData>> EditImage(IFormFile? image)
        {
            string img = null;
            ApplicationUser? user = await GETUSERBYTOKEN();
            if(image is not null && user.Profile_Picture is null)
            {
                img = await _cloudinary.UploadImageAsync(image);
			}
            else if(image is not null && user.Profile_Picture is not null)
            {
				string url = user.Profile_Picture;
				// Regular expression to match the ID part of the URL
				string pattern = @"\/([^\/]+)\.jpg$";
				Match match = Regex.Match(url, pattern);
				if (match.Success)
				{
					string ProfileID = match.Groups[1].Value;
					img = await _cloudinary.UpdateImageAsync(ProfileID, image);
				}
			}

			user.Profile_Picture = img;
            var result = await _user.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
            UserData profile = user;
            return Ok(profile);
        }
        private async Task<ApplicationUser?> GETUSERBYTOKEN()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _user.FindByEmailAsync(email ?? string.Empty);
			return user;
		}
        [HttpPost("resetPassword")]
        public async Task<ActionResult<bool>> ResetPassword(ResetOldPasswordDTO DTO) 
        {
			ApplicationUser? user = await GETUSERBYTOKEN();
			var Result = await _signIn.CheckPasswordSignInAsync(user, DTO.OldPassword, false);
            if(!Result.Succeeded) return BadRequest(new ApiResponse(401));
			var resetToken = await _user.GeneratePasswordResetTokenAsync(user);
			var result =await _user.ResetPasswordAsync(user, resetToken, DTO.NewPassword);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));
			return Ok(true);
		}
		
	}
}
