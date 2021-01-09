using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        readonly ICatalogApi catalogApi;
        readonly IBasketApi basketApi;

        public ProductModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            this.catalogApi = catalogApi;
            this.basketApi = basketApi;
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var productList = await catalogApi.GetCatalog();
            CategoryList = productList.Select(x => x.Category).Distinct();
            if (!string.IsNullOrEmpty(categoryName))
            {
                ProductList = productList.Where(x => x.Category == categoryName);
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = productList;
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