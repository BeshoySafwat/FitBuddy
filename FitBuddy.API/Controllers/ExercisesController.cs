using FitBuddy.API.Helper.DTO.ExercisesDTOS;
using FitBuddy.Core.Entities;
using FitBuddy.Core.Repositroy.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace FitBuddy.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ExercisesController : ControllerBase
	{
		private readonly IExercisesRepository _ex;
		private readonly UserManager<ApplicationUser> _user;

		public ExercisesController(IExercisesRepository ex
			,UserManager<ApplicationUser> user)
		{
			_ex = ex;
			_user = user;
		}

		[HttpPost("AddExercises")]
		public async Task<ActionResult<bool>> AddExercises(ExercisesInDTO DTO)
		{
			ApplicationUser user = await GETUSERBYTOKEN();
			Exercises exercises = DTO;
			exercises.U_ID = user.Id;
			Dictionary<string, int> frequency = new Dictionary<string, int>();

			// Count occurrences
			foreach (string item in DTO.ExercisesMessages)
			{
				if (frequency.ContainsKey(item))
					frequency[item]++;
				else
					frequency[item] = 1;
			}

			var value =string.Join(", ", frequency.Select(kv => $"{kv.Key} x{kv.Value}"));
			exercises.Messages = value;
			_ex.Add(exercises);
			return Ok(true);
		}

		[HttpGet("ExercisesCount")]
		public async Task<ActionResult<int>> GetCount()
		{
			ApplicationUser user = await GETUSERBYTOKEN();
			var count =_ex.GetExerciesCount(user.Id);
			return Ok(count);
		}
		[HttpGet("ExercisesPoints")]
		public async Task<ActionResult<int>> GetPoints()
		{
			ApplicationUser user = await GETUSERBYTOKEN();
			var point =_ex.GetExerciesPoints(user.Id);
			return Ok(point);
		}
		[HttpGet("Report")]
		public async Task<ActionResult<List<object>>> Report()
		{
			ApplicationUser user = await GETUSERBYTOKEN();
			var point =_ex.GetReport(user.Id);
			return Ok(point);
		}

		private async Task<ApplicationUser?> GETUSERBYTOKEN()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _user.FindByEmailAsync(email ?? string.Empty);
			return user;
		}
	}
}
