using FitBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Repositroy.Contract
{
	public interface IExercisesRepository:IGenericRepositroy<Exercises>
	{
		int GetExerciesCount(string userID);
		int GetExerciesPoints(string userID);
		List<(ApplicationUser User, int TotalPoints)> GetTops();
		List<object> GetReport(string userID);
	}
}
