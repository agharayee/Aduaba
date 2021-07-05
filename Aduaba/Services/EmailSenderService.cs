using Aduaba.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly string _fromName;
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
            _fromEmail = _configuration["SendGrid:FromEmail"];
            _fromName = _configuration["SendGrid:FromName"];
            _apiKey = _configuration["SendGrid:ApiKey"];
        }
        public async Task SendEmailAsync(string email, string subject, string Message)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = subject,
                PlainTextContent = Message,
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}
