using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IEmailSender
    {
        string SendEmailAsync(string email, string subject, string Message);
    }

}
