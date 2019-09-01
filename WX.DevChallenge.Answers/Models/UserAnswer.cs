using Newtonsoft.Json;

namespace WX.DevChallenge.Answers.Models
{
    public class UserAnswer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
