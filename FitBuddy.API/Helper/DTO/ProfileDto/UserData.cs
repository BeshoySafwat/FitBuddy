using FitBuddy.Core.Entities;

namespace FitBuddy.API.Helper.DTO.ProfileDto
{
    public class UserData
    {
        public string Image { get; set; } = null;
        public string DisplayName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string Gender { get; set; }
        public decimal? Tall { get; set; }
        public decimal? Weight { get; set; }

        public static implicit operator UserData(ApplicationUser user)
        {
            return new UserData()
            {
                Image =user.Profile_Picture,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                Age = user.Age,
                Gender = user.Gender.ToString(),
                Tall = user.Tall,
                Weight = user.Weight
            };
        }
    }
}
