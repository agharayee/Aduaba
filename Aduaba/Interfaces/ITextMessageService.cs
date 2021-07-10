using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vonage.Messaging;

namespace Aduaba.Interfaces
{
    public interface ITextMessageService
    {
        SendSmsResponse SendMessage(string to, string body);
    }
}
