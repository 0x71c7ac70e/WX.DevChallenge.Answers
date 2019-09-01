using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WX.DevChallenge.Answers.Models;

namespace WX.DevChallenge.Answers.Services
{
    public class TrolleyTotalService
    {
        // find the combination of specials and normal price that would result in lowest price
        // can apply a special as many times as possible while there is enough quantity (repeats possible)
        // the specials structure will always include all products; some products can have 0 quantity for that special
        // TODO: there's a much better algorithm out there but this is better than nothing
        public Decimal GetLowestTotal(ProductsSpecialsQuantities productsSpecialsQuantities)
        {
            Decimal? lowest = null;
            var prodPrices = productsSpecialsQuantities.Products;
            var prodQuants = productsSpecialsQuantities.ProductQuantities.ToList();
            var specials = productsSpecialsQuantities.Specials.ToList();

            // try a simulation such that the order of specials applied changes each run
            var numSimulations = specials.Count;

            for (int i = 0; i < numSimulations; i++)
            {
                Decimal simPrice = 0;

                var remaingProdQuants = new Dictionary<string, int>();
                foreach (var prodQuant in prodQuants)
                    remaingProdQuants.Add(prodQuant.Name, prodQuant.Quantity);

                foreach (var special in specials)
                {
                    // specials can be applied repeatedly
                    while (special.Quantities.All((q => q.Quantity <= remaingProdQuants.GetValueOrDefault(q.Name))))
                    {
                        foreach (var prodQuant in special.Quantities)
                        {
                            remaingProdQuants[prodQuant.Name] -= prodQuant.Quantity;
                        }
                        simPrice += special.Total;
                    }
                }

                // anything left over use the normal price
                foreach (var item in remaingProdQuants)
                {
                    var remainingQuant = item.Value;
                    if (remainingQuant > 0)
                    {
                        var prod = prodPrices.FirstOrDefault(p => p.Name == item.Key);
                        simPrice += prod.Price * remainingQuant;
                    }
                }

                var first = specials[0];
                specials.RemoveAt(0);
                specials.Add(first);

                if (lowest == null || simPrice < lowest)
                    lowest = simPrice;
            }

            return lowest.HasValue ? lowest.Value : 0;
        }
    }
}
