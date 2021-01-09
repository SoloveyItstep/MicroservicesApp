using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class OrderModel : PageModel
    {
        readonly IOrderApi orderApi;

        public OrderModel(IOrderApi orderApi)
        {
            this.orderApi = orderApi;
        }

        public IEnumerable<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            Orders = await orderApi.GetOrdersByUserName("swn").ConfigureAwait(false);

            return Page();
        }       
    }
}