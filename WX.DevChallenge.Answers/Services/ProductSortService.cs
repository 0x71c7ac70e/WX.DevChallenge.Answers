using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WX.DevChallenge.Answers.Models;

namespace WX.DevChallenge.Answers.Services
{
    public class ProductSortService
    {
        private readonly HelperResourceService _helperResourceService;

        public ProductSortService(HelperResourceService helperResourceService) =>_helperResourceService = helperResourceService;

        public IEnumerable<Product> Sort(IEnumerable<Product> products, SortOption sortOption)
        {
            SortStrategy sortStrategy = null;
            switch (sortOption)
            {
                case SortOption.Low: sortStrategy = new LowSortStrategy(); break;
                case SortOption.High: sortStrategy = new HighSortStrategy(); break;
                case SortOption.Ascending: sortStrategy = new AscendingSortStrategy(); break;
                case SortOption.Descending: sortStrategy = new DescendingSortStrategy(); break;
                case SortOption.Recommended:
                    {
                        var shopperHistory = Task.Run(() => _helperResourceService.GetShopperHistory()).Result;
                        sortStrategy = new RecommendedSortStrategy(shopperHistory);
                        break;
                    }
                    
                default: break;
            }

            if (sortStrategy == null)
                return products;

            return sortStrategy.Sort(products);
        }

        public abstract class SortStrategy
        {
            public abstract IEnumerable<Product> Sort(IEnumerable<Product> products);
        }

        public class LowSortStrategy : SortStrategy
        {
            public override IEnumerable<Product> Sort(IEnumerable<Product> products)
            {
                return products.OrderBy(p => p.Price);
            }
        }

        public class HighSortStrategy : SortStrategy
        {
            public override IEnumerable<Product> Sort(IEnumerable<Product> products)
            {
                return products.OrderByDescending(p => p.Price);
            }
        }

        public class AscendingSortStrategy : SortStrategy
        {
            public override IEnumerable<Product> Sort(IEnumerable<Product> products)
            {
                return products.OrderBy(p => p.Name);
            }
        }

        public class DescendingSortStrategy : SortStrategy
        {
            public override IEnumerable<Product> Sort(IEnumerable<Product> products)
            {
                return products.OrderByDescending(p => p.Name);
            }
        }

        public class RecommendedSortStrategy : SortStrategy
        {
            private readonly IEnumerable<ShopperPurchase> _shopperHistory;

            public RecommendedSortStrategy(IEnumerable<ShopperPurchase> shopperHistory) : base() => _shopperHistory = shopperHistory;

            public override IEnumerable<Product> Sort(IEnumerable<Product> products)
            {
                // keep a running count per product via dictionary
                var productsTotalDict = new Dictionary<Product, double>(new ProductEqualityComparer());
                foreach (var product in products)
                    productsTotalDict.Add(product, 0);
                foreach (var shopperPurchase in _shopperHistory)
                {
                    foreach (var purchaseProduct in shopperPurchase.Products)
                    {
                        double total;
                        if (productsTotalDict.TryGetValue(purchaseProduct, out total))
                        {
                            productsTotalDict[purchaseProduct] = total + purchaseProduct.Quantity;
                        }
                    }
                }

                // sort by the dictionary value (the total)
                return 
                    from entry in productsTotalDict
                    orderby entry.Value descending
                    select entry.Key;
            }

            public class ProductEqualityComparer : IEqualityComparer<Product>
            {
                public bool Equals(Product a, Product b)
                {
                    return a.Name.Equals(b.Name);
                }

                public int GetHashCode(Product obj)
                {
                    return obj.Name.GetHashCode();
                }
            }
        }
    }


}
