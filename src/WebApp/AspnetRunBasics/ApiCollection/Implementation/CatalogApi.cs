using AspnetRunBasics.ApiCollection.Abstract;
using AspnetRunBasics.ApiCollection.Infrastracture;
using AspnetRunBasics.Models;
using AspnetRunBasics.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Implementation
{
    public class CatalogApi : BaseHttpClientWithFactory, ICatalogApi
    {
        readonly IApiSettings settings;

        public CatalogApi(IHttpClientFactory factory, IApiSettings settings)
            :base(factory)
        {
            this.settings = settings;
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.CatalogPath)
                .HttpMethod(HttpMethod.Post)
                .GetHttpMessage();
            var json = JsonConvert.SerializeObject(model);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return await SendRequest<CatalogModel>(message).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.CatalogPath)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await SendRequest<IEnumerable<CatalogModel>>(message).ConfigureAwait(false);
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.CatalogPath)
                .AddToPath(id)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await SendRequest<CatalogModel>(message).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategoty()
        {
            var message = new HttpRequestBuilder(settings.BaseAddress)
                .SetPath(settings.CatalogPath)
                .AddToPath("GetProductByCategory")
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();
            return await SendRequest<IEnumerable<CatalogModel>>(message).ConfigureAwait(false);
        }
    }
}
