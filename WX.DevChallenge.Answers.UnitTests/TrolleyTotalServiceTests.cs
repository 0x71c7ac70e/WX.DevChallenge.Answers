using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WX.DevChallenge.Answers.Models;
using WX.DevChallenge.Answers.Services;

namespace Tests
{
    [TestFixture]
    public class TrolleyTotalServiceTests
    {
        // input has specials that can be used repeatedly
        [Test]
        public void ReusableSpecials()
        {
            var productsSpecialsQuantities = GetProductsSpecialsQuantities("productsSpecialsQuantities1.json");
            var service = new TrolleyTotalService();
            Assert.AreEqual(service.GetLowestTotal(productsSpecialsQuantities), 14);
        }

        // input has multiple different specials combined together
        [Test]
        public void UseMultipleSpecials()
        {
            var productsSpecialsQuantities = GetProductsSpecialsQuantities("productsSpecialsQuantities2.json");
            var service = new TrolleyTotalService();
            Assert.AreEqual(service.GetLowestTotal(productsSpecialsQuantities), 10);
        }

        // input has multiple combinations that end up with the same lowest price
        [Test]
        public void CombinationsWithEqualLowest()
        {
            var productsSpecialsQuantities = GetProductsSpecialsQuantities("productsSpecialsQuantities3.json");
            var service = new TrolleyTotalService();
            Assert.AreEqual(service.GetLowestTotal(productsSpecialsQuantities), 8);
        }

        // input has specials that can't be used as their quantity is bigger than the required quantities
        [Test]
        public void UnusedSpecials()
        {
            var productsSpecialsQuantities = GetProductsSpecialsQuantities("productsSpecialsQuantities4.json");
            var service = new TrolleyTotalService();
            Assert.AreEqual(service.GetLowestTotal(productsSpecialsQuantities), 13.8);
        }

        // input has specials that doesn't cover the full quantities required
        [Test]
        public void SpecialNotEnough()
        {
            var productsSpecialsQuantities = GetProductsSpecialsQuantities("productsSpecialsQuantities5.json");
            var service = new TrolleyTotalService();
            Assert.AreEqual(service.GetLowestTotal(productsSpecialsQuantities), 25.3);
        }

        private ProductsSpecialsQuantities GetProductsSpecialsQuantities(string fileName)
        {
            var productsJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\" + fileName);
            return JsonConvert.DeserializeObject<ProductsSpecialsQuantities>(productsJson);
        }
    }
}
