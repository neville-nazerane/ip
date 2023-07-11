using System.Net;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Ip.Server
{
    public class HttpCalls
    {
        private readonly ILogger _logger;
        private static readonly HttpClient client = new HttpClient();

        public HttpCalls(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpCalls>();
        }


        [Function("GetIPInfo")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ip")] HttpRequestData req)
        {

            string ipAddress;
            if (req.Headers.TryGetValues("X-Forwarded-For", out var headerValues))
            {
                ipAddress = headerValues.FirstOrDefault()
                                        .Split(',')
                                        .First()
                                        .Split(':')
                                        .First();
            }
            else
            {
                ipAddress = "Unknown";
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(ipAddress);

            return response;
        }

    }
}
