using Microsoft.AspNetCore.Mvc;
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
    public class AnswersController : Controller
    {
        private readonly ChallengeConfig _challengeConfig;
        private readonly ProductSortService _productSortService;
        private readonly HelperResourceService _helperResourceService;

        public AnswersController(
            ChallengeConfig challengeConfig,
            ProductSortService productSortService,
            HelperResourceService helperResourceService) 
            : base()
        {
            _challengeConfig = challengeConfig;
            _productSortService = productSortService;
            _helperResourceService = helperResourceService;
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
    }
}
