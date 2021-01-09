using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        readonly ICatalogApi catalogApi;
        readonly IBasketApi basketApi;

        public CheckOutModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            this.catalogApi = catalogApi;
            this.basketApi = basketApi;
        }

        [BindProperty]
        public BasketCheckoutModel Order { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketApi.GetBasket("swn").ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await basketApi.GetBasket("swn").ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = "swn";
            Order.TotalPrice = Cart.TotalPrice;

            await basketApi.CheckoutBasket(Order).ConfigureAwait(false);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}