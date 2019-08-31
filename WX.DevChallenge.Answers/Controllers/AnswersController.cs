using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ChallengeConfig ChallengeConfig;

        public AnswersController(ChallengeConfig challengeConfig) : base() => ChallengeConfig = challengeConfig;

        [HttpGet("user")]
        public IActionResult UserAnswer()
        {
            return Json(new UserAnswer()
            {
                Name = ChallengeConfig.Name,
                Token = ChallengeConfig.Token
            });
        }
    }
}
