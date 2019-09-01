using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WX.DevChallenge.Answers.Models;
using WX.DevChallenge.Answers.Security;
using WX.DevChallenge.Answers.Services;

namespace WX.DevChallenge.Answers.Controllers
{
    [Route("api/[controller]")]
    [RequireHttpsStrict]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AnswersController : Controller
    {
        private readonly ChallengeConfig _challengeConfig;
        private readonly ProductSortService _productSortService;
        private readonly HelperResourceService _helperResourceService;
        private readonly TrolleyTotalService _trolleyTotalService;
        private readonly ILogger<AnswersController> _logger;

        public AnswersController(
            ChallengeConfig challengeConfig,
            ProductSortService productSortService,
            HelperResourceService helperResourceService,
            TrolleyTotalService trolleyTotalService,
            ILogger<AnswersController> logger) 
            : base()
        {
            _challengeConfig = challengeConfig;
            _productSortService = productSortService;
            _helperResourceService = helperResourceService;
            _trolleyTotalService = trolleyTotalService;
            _logger = logger;
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
            SortOption sortingOption;
            if (!Enum.TryParse(sortOption, out sortingOption))
                return BadRequest("Invalid sort option");

            var products = await _helperResourceService.GetProducts();

            var sortedProducts = _productSortService
                .Sort(products, sortingOption)
                .ToList();

            return Json(sortedProducts);
        }

        [HttpPost("trolleyTotal")]
        public async Task<IActionResult> TrolleyTotalAnswer()
        {
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                //_logger.LogInformation(body);

                var productsSpecialsQuantities = JsonConvert.DeserializeObject<ProductsSpecialsQuantities>(body);
                return Ok(_trolleyTotalService.GetLowestTotal(productsSpecialsQuantities));

                //return Ok(await _helperResourceService.PostTrolleyTotal(body));
            }
        }
    }
}
