using Aduaba.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly string _fromName;
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _userName;
        private readonly string _Password;
        private readonly string _jwt;
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;

            _fromEmail = _configuration["MailTrap:FromEmail"];
            _fromName = _configuration["MailTrap:FromName"];
            _apiKey = _configuration["MailTrap:ApiKey"];
            _userName = _configuration["MailTrap:Username"];
            _Password = _configuration["MailTrap:Password"];
            _jwt = _configuration["MailTrap:Jwt"];
        }
        public string SendEmailAsync(string email, string subject, string Message)
        {
            
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress(_fromEmail);
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = Message;

            var client = new SmtpClient("smtp.pepipost.com", 2525)
            {
                EnableSsl =true,
                Credentials = new NetworkCredential(_userName, _Password),
            };
            
           
             client.Send(mail);
            return "Email Sent Successfully";
        }
    }
}
