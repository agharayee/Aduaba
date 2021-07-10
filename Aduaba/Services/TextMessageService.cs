using Aduaba.Interfaces;
using Microsoft.Extensions.Configuration;
using Vonage.Verify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Aduaba.Services
{
    public class TextMessageService : ITextMessageService
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _from;
        private readonly string _brand;
        public TextMessageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["nexmo:ApiKey"];
            _apiSecret = _configuration["nexmo:ApiSecret"];
            _from = _configuration["nexmo:from"];
            _brand = _configuration["nexmo:Brand"];
        }
        public SendSmsResponse SendMessage(string to, string body)
        {
            var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
            var client = new SmsClient
            {
                Credentials = credentials
            };
            var request = new SendSmsRequest
            {
                To = to,
                From = _brand,
                Text = body,
            };
            return client.SendAnSms(request);
        }

        public void SendOtp(string to)
        {
            var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
            var client = new VonageClient(credentials);

            var request = new VerifyRequest() { Brand = _brand, Number = to };
            var response = client.VerifyClient.VerifyRequest(request);
        }

        //public void VerifySentOtp(string code)
        //{
        //    var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
        //    var client = new VonageClient(credentials);
        //    var request = new VerifyCheckRequest() { Code = code, RequestId = REQUEST_ID };
        //    var response = client.VerifyClient.VerifyCheck(request);
        //}
    }
}
