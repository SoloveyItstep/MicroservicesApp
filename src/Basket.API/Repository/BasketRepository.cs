using Basket.API.Data.Abstract;
using Basket.API.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        readonly IBasketContext context;

        public BasketRepository(IBasketContext basketContext)
        {
            this.context = basketContext;
        }

        public Task<bool> DeleteBasket(string userName)
        {
            return context.Redis.KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await context.Redis.StringGetAsync(userName).ConfigureAwait(false);
            return basket.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<BasketCart>(basket); 
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var update = await context.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket)).ConfigureAwait(false);
            return update ? await GetBasket(basket.UserName).ConfigureAwait(false) : null;
        }
    }
}
