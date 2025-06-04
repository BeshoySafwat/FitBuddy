using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Entities
{
    public enum Gender
    {
        Male = 0,
        Female = 1,
    }
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public string? Profile_Picture { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public decimal? Tall { get; set; }
        public decimal? Weight { get; set; }
        public string? Address { get; set; }
        // External provider IDs
        public string? GoogleId { get; set; } = null!;
        public string? FacebookId { get; set; } = null!;

        public ICollection<Exercises>? Exercises { get; set; } = new HashSet<Exercises>();
    }
}
