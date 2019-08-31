using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WX.DevChallenge.Answers.Models;
using WX.DevChallenge.Answers.Security;

namespace WX.DevChallenge.Answers.Controllers
{
    [Route("api/[controller]")]
    [RequireHttpsStrict]
    public class AnswersController : Controller
    {
        private readonly ChallengeConfig _challengeConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnswersController(ChallengeConfig challengeConfig, IHttpClientFactory httpClientFactory) : base()
        {
            _challengeConfig = challengeConfig;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("user")]
        public IActionResult UserAnswer()
        {
            return Json(new UserAnswer()
            {
                Name = _challengeConfig.Name,
                Token = _challengeConfig.Token
            });
        }

        [HttpGet("sort")]
        public async Task<IActionResult> SortAnswer(string sortOption)
        {
            // make sure sort option is valid
            SortOption option;
            if (!Enum.TryParse(sortOption, out option))
                return BadRequest("Invalid sort option");

            // get list of products from help resource
            var builder = new UriBuilder(_challengeConfig.HelperResourceBaseUrl);
            builder.Path += "products";
            builder.Query = "token=" + _challengeConfig.Token;

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            request.Headers.Add("Accept", "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving products");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var productsList = JsonConvert.DeserializeObject<IList<Product>>(jsonContent);

            return Ok();

        }
    }
}
