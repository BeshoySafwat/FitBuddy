using FitBuddy.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Infrastructure.DBContext
{
    public class StoreDbContext :IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Exercises> Exercises { get; set; }
    }
}
