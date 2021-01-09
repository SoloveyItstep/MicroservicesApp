using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Infrastracture
{
    public class HttpRequestBuilder
    {
        readonly HttpRequestMessage request;
        string baseAddress;
        readonly ApiBuilder apiBuilder;

        public HttpRequestBuilder(string uri)
            :this(new ApiBuilder(uri))
        { }

        public HttpRequestBuilder(ApiBuilder apiBuilder)
        {
            this.request = new HttpRequestMessage();
            this.apiBuilder = apiBuilder;
            this.baseAddress = apiBuilder.GetLeftPart();
        }

        public HttpRequestBuilder AddToPath(string path)
        {
            apiBuilder.AddToPath(path);
            request.RequestUri = apiBuilder.GetUri();
            return this;
        }

        public HttpRequestBuilder SetPath(string path)
        {
            apiBuilder.SetPath(path);
            request.RequestUri = apiBuilder.GetUri();
            return this;
        }

        public HttpRequestBuilder HttpMethod(HttpMethod httpMethod)
        {
            request.Method = httpMethod;
            return this;
        }

        public HttpRequestBuilder Headers(Action<HttpRequestHeaders> funcOfHeaders)
        {
            funcOfHeaders(request.Headers);
            return this;
        }

        public HttpRequestBuilder Headers(NameValueCollection headers)
        {
            request.Headers.Clear();
            foreach (var item in headers.AllKeys)
            {
                request.Headers.Add(item, headers[item]);
            }
            return this;
        }

        public HttpRequestBuilder Content(HttpContent content)
        {
            request.Content = content;
            return this;
        }

        public HttpRequestBuilder RequestUri(Uri uri)
        {
            request.RequestUri = new ApiBuilder(uri.ToString())
                .GetUri();
            return this;
        }

        public HttpRequestBuilder RequestUri(string uri)
        {
            return RequestUri(new Uri(uri));
        }

        public HttpRequestBuilder BaseAddress(string address)
        {
            this.baseAddress = address;
            return this;
        }

        public HttpRequestBuilder Subdomain(string subdomain)
        {
            apiBuilder.SetSubdomain(subdomain);
            request.RequestUri = apiBuilder.GetUri();
            return this;
        }

        public HttpRequestBuilder AddQueryString(string name, string value)
        {
            apiBuilder.AddQueryString(name, value);
            request.RequestUri = apiBuilder.GetUri();
            return this;
        }

        public HttpRequestBuilder SetQueryString(string queryString)
        {
            apiBuilder.QueryString(queryString);
            request.RequestUri = apiBuilder.GetUri();
            return this;
        }

        public HttpRequestMessage GetHttpMessage()
        {
            return request;
        }

        public ApiBuilder GetApiBuilder()
        {
            return new ApiBuilder(request.RequestUri.ToString());
        }
    }
}
