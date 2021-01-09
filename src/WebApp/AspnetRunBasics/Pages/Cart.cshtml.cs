using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        readonly IBasketApi basketApi;

        public CartModel(IBasketApi basketApi)
        {
            this.basketApi = basketApi;
        }


        public BasketModel Cart { get; set; } = new BasketModel();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketApi.GetBasket("swn").ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var basket = await basketApi.GetBasket("swn").ConfigureAwait(false);
            var item = basket.Items.Single(x => x.ProductId == productId);
            basket.Items.Remove(item);

            var basketUpdate = await basketApi.UpdateBasket(basket).ConfigureAwait(false);
            return RedirectToPage();
        }
    }
}