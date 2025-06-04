using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Services.Contract
{
    public interface IEmailService
    {
        public Task SendAsync(string From, string Recipients, string Subject, string Body);
    }
}
