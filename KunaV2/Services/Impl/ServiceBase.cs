using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KunaV2.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KunaV2.Services.Impl
{
    public abstract class ServiceBase
    {
        protected readonly IRestClient Client;
        private readonly ILogger<ServiceBase> _logger;

        protected ServiceBase(ILogger<ServiceBase> logger, IRestClient client)
        {
            _logger = logger;
            Client = client;
        }

        protected IRestRequest CreateSignedRequest(Method verb, string endpoint, string market = null,
            object body = null)
        {
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            var paramString = $"access_key={Config.ApiKey.Public}" +                            //access_key
                              (!string.IsNullOrWhiteSpace(market) ? $"&market={market}" : "") + //market
                              $"&tonce={time}";                                                 //tonce

            byte[] hash;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Config.ApiKey.Private)))
            {
                var input = $"{verb.ToString().ToUpper()}|/api/v2/{endpoint}|{paramString}";
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            var key = BitConverter.ToString(hash).Replace("-", "").ToLower();
            paramString += $"&signature={key}";

            var request = new RestRequest($"{endpoint}?{paramString}");
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.Method = verb;

            if (body != null)
                request.AddJsonBody(body);

            return request;
        }

        protected async Task<T> ExecuteRequestWithRetry<T>(Method verb, string endpoint, string market = null,
            object body = null) where T : class
        {
            var attempt = -1;
            const int maxAttemptCount = 10;
            IRestResponse<T> response;

            do
            {
                attempt++;

                response = await Client.ExecuteAsync<T>(CreateSignedRequest(verb, endpoint, market, body));

                if (response.IsSuccessful)
                    return response.Data;

                if(IsSuccessful(response.StatusCode) && response.ResponseStatus == ResponseStatus.Error)
                    throw new Exception("Response status code successful but response status error. " 
                                             + response.ErrorMessage, response.ErrorException);

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    await TolerateThrottlingError(verb, endpoint, response, attempt);
                else
                    TolerateMiscErrors(verb, endpoint, response, attempt);

            } while (attempt < maxAttemptCount);

          
            throw new Exception($"\n{response.StatusCode}\n{response.Content}\n{response.ErrorMessage}");
        }

        private static bool IsSuccessful(HttpStatusCode code) => code >= HttpStatusCode.OK && code < HttpStatusCode.Ambiguous;

        private async Task TolerateThrottlingError(Method verb, string endpoint, IRestResponse response, int attempt)
        {
            _logger.LogWarning($"Response from {verb} {endpoint} returned code {response.StatusCode}. " +
                               $"Attempt {attempt}. Trying to tolerate throttling error.");

            var retryAfterHeader = response.Headers?.FirstOrDefault(h => h?.Name?.ToLower() == "retry-after");
            if (retryAfterHeader == null)
            {
                _logger.LogWarning("Retry-After header is missing. Waiting arbitrarily 5 seconds.");
                await Task.Delay(5000);
                return;
            }

            const int tradeWorkerInterval = 60; //ToDo:: should be synchronized with the actual interval
            var timeToWaitInMs = Convert.ToInt32(retryAfterHeader.Value);
            if (TimeSpan.FromMilliseconds(timeToWaitInMs) <= TimeSpan.FromSeconds(tradeWorkerInterval))
            {
                _logger.LogWarning($"Retry-After header value is {timeToWaitInMs}. Waiting.");
                await Task.Delay(timeToWaitInMs);
                return;
            }

            _logger.LogWarning($"Retry-After header value is {timeToWaitInMs} " +
                               $"which exceeds trade worker interval of {tradeWorkerInterval} sec. " +
                               "Breaking.");

            throw new Exception("ThrottlingException");
        }


        private void TolerateMiscErrors(Method verb, string endpoint, IRestResponse response, int attempt)
        { 
            const int timeToWaitInSec = 1;
            _logger.LogWarning($"Response from {verb} {endpoint} returned code {response.StatusCode}. " +
                               $"Attempt {attempt}. Content: {response.Content}; " +
                               $"Error message {response.ErrorMessage}. " +
                               $"Waiting {timeToWaitInSec} sec. For retry.");

            Thread.Sleep(TimeSpan.FromSeconds(timeToWaitInSec));
        }
    }
}
