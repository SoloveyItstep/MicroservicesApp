using System;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        readonly ICatalogApi catalogApi;
        readonly IBasketApi basketApi;

        public ProductDetailModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            this.catalogApi = catalogApi;
            this.basketApi = basketApi;
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }

            Product = await catalogApi.GetCatalog(productId);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await catalogApi.GetCatalog(productId).ConfigureAwait(false);
            var userName = "swn";
            var basket = await basketApi.GetBasket(userName).ConfigureAwait(false);
            basket.Items.Add(new BasketItemModel
            {
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