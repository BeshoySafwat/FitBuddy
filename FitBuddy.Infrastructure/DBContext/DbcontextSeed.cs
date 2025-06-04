using FitBuddy.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Infrastructure.DBContext
{
    public static class DbcontextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    Email = "beshoysafwat556@gmail.com",
                    DisplayName = "Beshoy Safwat",
                    UserName = "beshoysafwat",
                    PhoneNumber = "01280563119",
                    Gender = Gender.Male,

                };
                await userManager.CreateAsync(user, "Beshoy_00");
            }
        }
    }
}
