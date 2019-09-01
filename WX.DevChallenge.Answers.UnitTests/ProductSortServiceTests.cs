using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WX.DevChallenge.Answers.Models;
using WX.DevChallenge.Answers.Services;

namespace Tests
{
    // note: only tests for the Recommended sort is here since the others basically just call Enumerable.OrderBy
    [TestFixture]
    public class ProductSortServiceTests
    {
        [Test]
        public void RecommendedSort_AllUnique()
        {
            var products = GetProducts().ToList();
            var shopperHistory = GetShopperHistory("shopperHistory1.json");

            var sortStrat = new ProductSortService.RecommendedSortStrategy(shopperHistory);
            var sorted = sortStrat.Sort(products);
            var expected = new List<Product>()
            {
                products[0],
                products[1],
                products[2],
                products[4],
                products[3]
            };

            CollectionAssert.AreEqual(sorted, expected);
        }

        [Test]
        public void RecommendedSortTest_SomeEqual()
        {
            var products = GetProducts().ToList();
            var shopperHistory = GetShopperHistory("shopperHistory2.json");

            var sortStrat = new ProductSortService.RecommendedSortStrategy(shopperHistory);
            var sorted = sortStrat.Sort(products);
            var expected = new List<Product>()
            {
                products[1],
                products[2],
                products[0],
                products[4],
                products[3]
            };

            CollectionAssert.AreEqual(sorted, expected);
        }

        private IEnumerable<Product> GetProducts()
        {
            var productsJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\products.json");
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);
        }

        private IEnumerable<ShopperPurchase> GetShopperHistory(string fileName)
        {
            var shopperHistoryJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\" + fileName);
            return JsonConvert.DeserializeObject<IEnumerable<ShopperPurchase>>(shopperHistoryJson);
        }
    }
}