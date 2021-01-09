using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task Create(Product product)
        {
            await catalogContext.Products.InsertOneAsync(product).ConfigureAwait(false);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(x => x.Id, id);
            DeleteResult result = await catalogContext.Products.DeleteOneAsync(filter).ConfigureAwait(false);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            var products = await catalogContext.Products.FindAsync(x => x.Id == id).ConfigureAwait(false);
            return await products.FirstOrDefaultAsync().ConfigureAwait(false);

        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await catalogContext.Products.FindAsync(x => true).ConfigureAwait(false);
            return await products.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetproductsByCategoryName(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.AnyIn(x => x.Category,categoryName);
            var products = await catalogContext.Products.FindAsync(filter).ConfigureAwait(false);
            return await products.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetproductsByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(x => x.Name, name);
            var products = await catalogContext.Products.FindAsync(filter).ConfigureAwait(false);
            return await products.ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(Product product)
        {
            ReplaceOneResult result = await catalogContext.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product).ConfigureAwait(false);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
