using Newtonsoft.Json;

namespace WX.DevChallenge.Answers.Models
{
    public class ShopperPurchase
    {
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("products")]
        public Product[] Products { get; set; }
    }
}
