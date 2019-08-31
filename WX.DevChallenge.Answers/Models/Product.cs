﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WX.DevChallenge.Answers.Models
{
    public class Product
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }
    }
}