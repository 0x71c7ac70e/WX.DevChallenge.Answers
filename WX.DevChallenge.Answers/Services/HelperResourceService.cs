using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WX.DevChallenge.Answers.Models;

namespace WX.DevChallenge.Answers.Services
{
    /* Encapsulate the external WX helper resource api calls to methods of this class
     *
     */
    public class HelperResourceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ChallengeConfig _challengeConfig;

        public HelperResourceService(IHttpClientFactory httpClientFactory, ChallengeConfig challengeConfig)
        {
            _httpClientFactory = httpClientFactory;
            _challengeConfig = challengeConfig;
        }

        private async Task<string> SendJsonRequest(string finalPath, HttpMethod httpMethod, string content = null)
        {
            var builder = new UriBuilder(_challengeConfig.HelperResourceBaseUrl);
            builder.Path += finalPath;
            builder.Query = "token=" + _challengeConfig.Token;

            var request = new HttpRequestMessage(httpMethod, builder.Uri);
            request.Headers.Add("Accept", "application/json");
            if (!String.IsNullOrEmpty(content))
            {
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new AppException("Error retrieving helper resource: " + finalPath);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var jsonContent = await SendJsonRequest("products", HttpMethod.Get);
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonContent);
        }

        public async Task<IEnumerable<ShopperPurchase>> GetShopperHistory()
        {
            var jsonContent = await SendJsonRequest("shopperHistory", HttpMethod.Get);
            return JsonConvert.DeserializeObject<IEnumerable<ShopperPurchase>>(jsonContent);
        }

        public async Task<decimal> PostTrolleyTotal(string inputData)
        {
            var result = await SendJsonRequest("trolleyCalculator", HttpMethod.Post, inputData);
            return Convert.ToDecimal(result);
        }
    }
}
