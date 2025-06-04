using FitBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync(ApplicationUser user);
    }
}
