using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.ApiCollection.Infrastracture;
using AspnetRunBasics.Models;
using AspnetRunBasics.Settings;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Implementation
{
    public class OrderApi : BaseHttpClientWithFactory, IOrderApi
    {
        readonly IApiSettings settings;

        public OrderApi(IHttpClientFactory factory, IApiSettings settings)
            : base(factory)
        {
            this.settings = settings;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.OrderPath)
                .AddQueryString("userName", userName)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await SendRequest<IEnumerable<OrderResponseModel>>(message).ConfigureAwait(false);
        }
    }
}
