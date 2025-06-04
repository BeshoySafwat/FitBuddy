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
    internal class ApplicationUserConfigration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> A)
        {
            A.Property(a => a.Gender)
           .HasConversion<string>();
            A.Property(a=>a.DisplayName).HasColumnType("nvarchar").HasMaxLength(150);
            A.Property(a=>a.Address).HasColumnType("nvarchar").HasMaxLength(300);
            A.Property(a => a.Tall).HasColumnType("decimal(18,2)");
            A.Property(a => a.Weight).HasColumnType("decimal(18,2)");
            A.HasMany(x=>x.Exercises)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.U_ID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
