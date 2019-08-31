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
        private readonly HelperResourceService _helperResourceService;

        public AnswersController(
            ChallengeConfig challengeConfig,
            HelperResourceService helperResourceService) 
            : base()
        {
            _challengeConfig = challengeConfig;
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

            return Ok();

        }
    }
}
