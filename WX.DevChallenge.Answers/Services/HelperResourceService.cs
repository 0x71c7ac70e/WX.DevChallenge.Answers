using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        private async Task<string> GetJsonResource(string finalPath)
        {
            var builder = new UriBuilder(_challengeConfig.HelperResourceBaseUrl);
            builder.Path += finalPath;
            builder.Query = "token=" + _challengeConfig.Token;

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            request.Headers.Add("Accept", "application/json");

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
            var jsonContent = await GetJsonResource("products");
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonContent);
        }

        public async Task<IEnumerable<ShopperPurchase>> GetShopperHistory()
        {
            var jsonContent = await GetJsonResource("shopperHistory");
            return JsonConvert.DeserializeObject<IEnumerable<ShopperPurchase>>(jsonContent);
        }
    }
}
