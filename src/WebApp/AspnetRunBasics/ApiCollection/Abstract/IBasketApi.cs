using AspnetRunBasics.Models;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Abstract
{
    public interface IBasketApi
    {
        Task<BasketModel> GetBasket(string userName);
        Task<BasketModel> UpdateBasket(BasketModel model);
        Task CheckoutBasket(BasketCheckoutModel model);

    }
}
