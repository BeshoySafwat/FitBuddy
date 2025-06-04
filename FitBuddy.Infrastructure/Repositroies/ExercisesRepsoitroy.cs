using FitBuddy.Core.Entities;
using FitBuddy.Core.Repositroy.Contract;
using FitBuddy.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Infrastructure.Repositroies
{
	public class ExercisesRepsoitroy : GenericRepositroy<Exercises>, IExercisesRepository
	{
		private readonly StoreDbContext _store;

		public ExercisesRepsoitroy(StoreDbContext store) : base(store)
		{
			_store = store;
		}

		public int GetExerciesCount(string userId)
		{
			var count =_store.Exercises.Where(x=>x.U_ID == userId).Count();
			return count;
		}
		public int GetExerciesPoints(string userId)
		{
			var points =_store.Exercises.Where(x=>x.U_ID == userId).Sum(x=>x.Points);
			return points;
		}

		public List<object> GetReport(string userID)
		{
			var report = _store.Exercises
				.Where(x => x.U_ID == userID)
				.Select(g => new
				{
						Date = g.DateTime,
						Name = g.Name,
						Messages = g.Messages
					
				}).ToList<object>();

			return report;
		}

		public List<(ApplicationUser User, int TotalPoints)> GetTops()
		{
			var topUsers = _store.Exercises
				.GroupBy(e => e.U_ID)
				.Select(g => new
				{
					User = g.First().User,
					TotalPoints = g.Sum(e => e.Points)
				})
				.OrderByDescending(g => g.TotalPoints)
				.Take(5)
				.ToList() 
				.Select(g => (g.User, g.TotalPoints)) 
				.ToList(); 

			return topUsers; 
		}

	}
}
