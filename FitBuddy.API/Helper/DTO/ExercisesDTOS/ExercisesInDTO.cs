using FitBuddy.Core.Entities;

namespace FitBuddy.API.Helper.DTO.ExercisesDTOS
{
	public class ExercisesInDTO
	{
		public string Name { get; set; }
		public int Points { get; set; } = 10;
		public DateOnly Day { get; set; } = DateOnly.FromDateTime(DateTime.Now);
		public decimal? Calories { get; set; }
		public List<string>? ExercisesMessages { get; set; }

		public static implicit operator Exercises(ExercisesInDTO dto)
		{
			return new Exercises
			{
				Name = dto.Name,
				Points = dto.Points,
				DateTime = dto.Day,
				Calories = dto.Calories

			};
		}

	}
}
