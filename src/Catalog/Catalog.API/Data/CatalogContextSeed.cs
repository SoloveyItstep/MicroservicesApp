using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedData(IMongoCollection<Product> productCollection)
        {
            var products = await productCollection.FindAsync(x => true).ConfigureAwait(false);
            if(!await products.AnyAsync().ConfigureAwait(false))
            {
                await productCollection.InsertManyAsync(GetPreconfigureProducts()).ConfigureAwait(false);
            }
        }

        private static IEnumerable<Product> GetPreconfigureProducts()
        {
            return new List<Product> { 
                new Product
                {
                    Name = "IPhone X",
                    Summary = "ssss",
                    Description = "ddddd",
                    Category = "Smart phones",
                    ImageFile = "product-1.png",
                    Price = 880.23M
                },
                new Product
                {
                    Name = "IPhone XX",
                    Summary = "wwww",
                    Description = "wwww",
                    Category = "Smart phones",
                    ImageFile = "product-2.png",
                    Price = 980.27M
                },
                new Product
                {
                    Name = "IPhone XXX",
                    Summary = "ssss",
                    Description = "ddddd",
                    Category = "Smart phones",
                    ImageFile = "product-3.png",
                    Price = 760.23M
                }
            };
        }
    }
}
