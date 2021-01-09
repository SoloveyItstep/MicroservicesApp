using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.ApiCollection.Infrastracture;
using AspnetRunBasics.Models;
using AspnetRunBasics.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Implementation
{
    public class BasketApi : BaseHttpClientWithFactory, IBasketApi
    {
        readonly IApiSettings settings;

        public BasketApi(IHttpClientFactory factory, IApiSettings settings)
            : base(factory)
        {
            this.settings = settings;
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.BasketPath)
                .AddToPath("Checkout")
                .HttpMethod(HttpMethod.Post)
                .GetHttpMessage();
            var json = JsonConvert.SerializeObject(model);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            await SendRequest<BasketCheckoutModel>(message).ConfigureAwait(false);
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.BasketPath)
                .AddQueryString("userName", userName)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await SendRequest<BasketModel>(message).ConfigureAwait(false);
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.BasketPath)
                .HttpMethod(HttpMethod.Post)
                .GetHttpMessage();
            var json = JsonConvert.SerializeObject(model);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<BasketModel>(message).ConfigureAwait(false);
        }
    }
}
