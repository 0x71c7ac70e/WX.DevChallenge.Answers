using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WX.DevChallenge.Answers.Models
{
    public class ProductsSpecialsQuantities
    {
        [JsonProperty("Products")]
        public Product[] Products { get; set; }

        [JsonProperty("Specials")]
        public Special[] Specials { get; set; }

        [JsonProperty("Quantities")]
        public ProductQuantity[] ProductQuantities { get; set; }


        public class Product
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Price")]
            public decimal Price { get; set; }
        }

        public class Special
        {
            [JsonProperty("Quantities")]
            public ProductQuantity[] Quantities { get; set; }

            [JsonProperty("Total")]
            public decimal Total { get; set; }
        }

        public class ProductQuantity
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Quantity")]
            public int Quantity { get; set; }
        }

    }
}
