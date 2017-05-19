using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Core
{
    public interface IEmailSender
    {
        Task SendEmail(string Email, string Subject, string Message);
    }
}
