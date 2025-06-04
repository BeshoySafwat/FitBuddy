using FitBuddy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Infrastructure.DBContext.Config
{
	public class ExercisesConfiguration : IEntityTypeConfiguration<Exercises>
	{
		public void Configure(EntityTypeBuilder<Exercises> E)
		{
			E.Property(x => x.Calories).HasColumnType("decimal(12,2)");
		}
	}
}
