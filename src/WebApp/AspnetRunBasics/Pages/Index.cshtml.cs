using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        readonly ICatalogApi catalogApi;
        readonly IBasketApi basketApi;

        public IndexModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            this.catalogApi = catalogApi;
            this.basketApi = basketApi;
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await catalogApi.GetCatalog().ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await catalogApi.GetCatalog(productId).ConfigureAwait(false);
            var userName = "swn";
            var basket = await basketApi.GetBasket(userName).ConfigureAwait(false);
            basket.Items.Add(new BasketItemModel { 
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            });

            var basketUpdated = await basketApi.UpdateBasket(basket).ConfigureAwait(false);
            return RedirectToPage("Cart");
        }
    }
}
