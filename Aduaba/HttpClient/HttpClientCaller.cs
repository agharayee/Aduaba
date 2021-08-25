using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aduaba.HttpClients
{
    public interface IHttpClientCaller
    {
        Task<string> HttpClientGateway(HttpClient client, string Url, HttpContent reqData);
        Task<string> HttpClientGatewayGet(HttpClient client, string Url);
        
           
    }
    public class HttpClientCaller : IHttpClientCaller
    {
        private readonly HttpClient client;
        private readonly ILogger<HttpClientCaller> logger;
        public HttpClientCaller(HttpClient client, ILogger<HttpClientCaller> logger)
        {
            this.client = client;
            this.logger = logger;
        }

      

        public async Task<string> HttpClientGatewayGet(HttpClient client, string Url)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = client.GetAsync(Url).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error occurred  - " + ex.Message + "||" + ex.InnerException);
                return null;
            }
        }

        public async Task<string> HttpClientGateway(HttpClient client, string Url, HttpContent reqData)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = client.PostAsync(Url, reqData).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Error occurred  - " + ex.Message + "||" + ex.InnerException);
                return null;
            }
        }


    }
}
