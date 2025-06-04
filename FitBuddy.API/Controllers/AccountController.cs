using ECommerce.API.Helper.Error;
using FitBuddy.API.Helper.DTO.Account;
using FitBuddy.Core.Entities;
using FitBuddy.Core.Services.Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitBuddy.API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;
        private readonly UserManager<ApplicationUser> _user;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly IEmailService _email;
        private readonly IConfiguration _configuration;

        public AccountController(IAuthService auth,
            UserManager<ApplicationUser> user,
            SignInManager<ApplicationUser> signIn,IEmailService email,IConfiguration configuration)
        {
            _auth = auth;
            _user = user;
            _signIn = signIn;
            _email = email;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO Dto)
        {
            var User = await _user.FindByEmailAsync(Dto.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var Result = await _signIn.CheckPasswordSignInAsync(User,Dto.Password,false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse( 401 ));

            return Ok(new UserDTO()
            {
                DisplayName =User.DisplayName,
                Email =User.Email,
                UserName = User.UserName,
                Token =await _auth.GetTokenAsync(User)
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO DTO)
        {
            var User = new ApplicationUser()
            {
                DisplayName = DTO.DisplayName,
                Email = DTO.Email,
                UserName = DTO.Email.Split("@")[0],
                Weight = DTO.Weight,
                Tall =DTO.Tall,
                Age = DTO.Age,
                Gender = DTO.Gender,
            };
            var result = await _user.FindByEmailAsync(DTO.Email);
            if (result is not null) return BadRequest(new ApiResponse(400,"This Email is already Token"));
            var CreateUser =await _user.CreateAsync(User,DTO.Password);
            if(CreateUser is null) return BadRequest(new ApiValidationResponse
            { Errors = CreateUser.Errors.Select(E => E.Description) });
            return Ok(new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                UserName=User.UserName,
                Token = await _auth.GetTokenAsync(User)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email ?? string.Empty);
            return Ok(new UserDTO()
            {
                Email = user?.Email ?? string.Empty,
                DisplayName = user?.DisplayName ?? string.Empty,
                UserName = user?.UserName ?? string.Empty,
                Token = await _auth.GetTokenAsync(user)
            });
        }
        [Authorize]
        [HttpPost("Logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            await _signIn.SignOutAsync();
            return Ok(true);
        }
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<bool>> ForgetPassword(ForgetPasswordDTO model)
        {
            var user = await _user.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                var Token = await _user.GeneratePasswordResetTokenAsync(user);
                var ResetPasswordURL = Url.Action("ResetPassword", "Account", new { user.Email, Token }, protocol: Request.Scheme);

                await _email.SendAsync(
                    From: _configuration["EmailSetting:EmailSender"],
                    Recipients: model.Email,
                    "Reset Password",
                    Body: ResetPasswordURL);
                return Ok(true);
            }
            return BadRequest(false);
        }
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string email, string Token)
        {
            TempData["Email"] = email;
            TempData["Token"] = Token;
            return View();
        }
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordDTO model)
		{
			if (ModelState.IsValid)
			{
				var Email = TempData["Email"] as string;
				var Token = TempData["Token"] as string;
				var user = await _user.FindByEmailAsync(Email);
				if (user is not null)
				{
					var result =await _user.ResetPasswordAsync(user, Token, model.NewPassword);
                    if (!result.Succeeded) return BadRequest(new ApiResponse(401));
                    return Ok("You Can Return to The App and Login, Free to Close the Window");
				}

				ModelState.AddModelError(string.Empty, "Email Is Not Vaild");
			}
			return View();
		}
		#region External Login
		[HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<ActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed");

            var googleUser = authenticateResult.Principal;
            var email = googleUser.FindFirstValue(ClaimTypes.Email);
            var name = googleUser.FindFirstValue(ClaimTypes.Name);
            var googleId = googleUser.FindFirstValue(ClaimTypes.NameIdentifier);
            // Get external login info
            var loginInfo = new UserLoginInfo(GoogleDefaults.AuthenticationScheme, googleId, "Google");

            // Check if the user already exists
            var user = await _user.FindByEmailAsync(email);

            if (user == null)
            {
                // Create a new user
                user = new ApplicationUser
                {
                    UserName = email.Split("@")[0],  // Unique username
                    Email = email,
                    DisplayName = name
                };

                var result = await _user.CreateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // Add external login to AspNetUserLogins
                var loginResult = await _user.AddLoginAsync(user, loginInfo);
                if (!loginResult.Succeeded)
                    return BadRequest(loginResult.Errors);
            }
            else
            {
                // Check if the user has an existing login with Google
                var existingLogins = await _user.GetLoginsAsync(user);
                var hasGoogleLogin = existingLogins.Any(l => l.LoginProvider == GoogleDefaults.AuthenticationScheme);

                if (!hasGoogleLogin)
                {
                    // Add Google login to the existing user
                    var loginResult = await _user.AddLoginAsync(user, loginInfo);
                    if (!loginResult.Succeeded)
                        return BadRequest(loginResult.Errors);
                }
            }
			if (user.PasswordHash != null && user.PasswordHash.Length > 0)
                return Ok("You Can Return to The App and Login, Free to Close the Window");

			var Token = await _user.GeneratePasswordResetTokenAsync(user);

			return RedirectToAction(nameof(ResetPassword),new {email=user.Email,token=Token});

        }
        #endregion

    }
}
