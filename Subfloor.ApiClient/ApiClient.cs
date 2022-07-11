using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using IdentityModel.Client;
using Subfloor.Dtos;

namespace Subfloor
{
    public class ApiClient
    {
        private HttpClient _client;
        private ILogger<ApiClient> _logger;

        public ApiClient(HttpClient client, ILogger<ApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public string Message { get; set; }

        private HttpRequestMessage GetHttpRequestMessage(HttpMethod httpMethod, string url, Object o = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            if (o != null && (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put || httpMethod == HttpMethod.Delete))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            }
            return request;
        }


        public void SetBearerToken(string token)
        {
            _client.SetBearerToken(token);
        }

        #region Transactions
        public async Task<Transaction> GetTransaction(Guid transactionId)
        {
            _logger.LogInformation("Getting Transaction {transactionId}", transactionId);
            var request = GetHttpRequestMessage(HttpMethod.Get, $"transactions/{transactionId}");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Transaction>();
            }
            else
            {
                Message = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}";
                _logger.LogError("Error getting transaction {transactionId}: {message}", transactionId, Message);
                return null;
            }
        }

        public async Task<Transaction> PostTransactionRequest(Transaction transaction)
        {
            _logger.LogInformation("Posting transaction to subfloor {@transaction}", transaction);
            var request = GetHttpRequestMessage(HttpMethod.Post, $"transactions", transaction);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Transaction>();
            }
            else
            {
                Message = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}";
                _logger.LogError("Error posting transaction request {@dto}: {message}", transaction, Message);
                return null;
            }
        }
        #endregion


        #region Subscribers (Requestors / Responders)
        public async Task<IEnumerable<Requestor>> GetRequestors()
        {
            _logger.LogInformation("Getting requestors");
            var request = GetHttpRequestMessage(HttpMethod.Get, $"requestors");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Requestor>>();
            }
            else
            {
                Message = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}";
                _logger.LogError("Error getting requestors: {message}", Message);
                return null;
            }
        }

        public async Task<IEnumerable<Responder>> GetResponders()
        {
            _logger.LogInformation("Getting responders");
            var request = GetHttpRequestMessage(HttpMethod.Get, $"responders");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Responder>>();
            }
            else
            {
                Message = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}";
                _logger.LogError("Error getting responders: {message}", Message);
                return null;
            }
        }
        #endregion
    }
}
