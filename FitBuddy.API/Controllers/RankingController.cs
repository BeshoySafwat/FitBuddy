using FitBuddy.API.Helper.DTO.ExercisesDTOS;
using FitBuddy.Core.Repositroy.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitBuddy.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RankingController : ControllerBase
	{
		private readonly IExercisesRepository _ex;

		public RankingController(IExercisesRepository ex)
		{
			_ex = ex;
		}

		[HttpGet]
		public async Task<ActionResult<List<TopUsersDTO>>> Top()
		{

			var Tops = _ex.GetTops();
			List<TopUsersDTO> result = new List<TopUsersDTO>();
			foreach (var Top in Tops)
			{
				result.Add(new TopUsersDTO()
				{
					Image = Top.User.Profile_Picture,
					Name = Top.User.DisplayName,
					Score = Top.TotalPoints
				});
			}
			return Ok(result);
		}
	}
}
