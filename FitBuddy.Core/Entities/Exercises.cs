using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Entities
{
	public class Exercises
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int Points { get; set; }
		public DateOnly DateTime { get; set; }
		public decimal? Calories { get; set; }
		public string? Messages { get; set; } 
		public string U_ID { get; set; }
		public ApplicationUser User { get; set; } 

	}
}
