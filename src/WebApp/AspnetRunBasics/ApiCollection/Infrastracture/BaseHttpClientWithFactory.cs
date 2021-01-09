using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Infrastracture
{
    public class BaseHttpClientWithFactory
    {
        readonly IHttpClientFactory factory;
        public Uri BaseAddress { get; set; }
        public string BasePath { get; set; }

        public BaseHttpClientWithFactory(IHttpClientFactory factory) => 
            this.factory = factory;

        private HttpClient GetHttpClient() => factory.CreateClient();

        public virtual async Task<T> SendRequest<T>(HttpRequestMessage request) where T: class
        {
            var client = GetHttpClient();
            var response = await client.SendAsync(request).ConfigureAwait(false);
            T result = null;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<T>(GetFormatters());
            return result;
        }

        protected virtual IEnumerable<MediaTypeFormatter> GetFormatters()
        {
            return new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() };
        }
    }
}
