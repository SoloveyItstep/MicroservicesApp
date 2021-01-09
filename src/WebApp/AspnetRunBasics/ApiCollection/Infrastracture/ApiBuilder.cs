using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspnetRunBasics.ApiCollection.Infrastracture
{
    public class ApiBuilder
    {
        readonly string fullUrl;
        UriBuilder builder;

        public ApiBuilder(string url)
        {
            this.fullUrl = url;
            this.builder = new UriBuilder(url);
        }

        public Uri GetUri() => builder.Uri;

        public ApiBuilder Scheme(string scheme)
        {
            builder.Scheme = scheme;
            return this;
        }

        public ApiBuilder Host(string host)
        {
            builder.Host = host;
            return this;
        }

        public ApiBuilder Port(int port)
        {
            builder.Port = port;
            return this;
        }

        public ApiBuilder AddToPath(string path)
        {
            IncludePath(path);
            return this;
        }

        public ApiBuilder SetPath(string path)
        {
            builder.Path = path;
            return this;
        }

        public void IncludePath(string path)
        {
            if (string.IsNullOrEmpty(builder.Path) || builder.Path == "/")
                builder.Path = path;
            else
            {
                var newPath = $"{builder.Path}/{path}";
                builder.Path = newPath.Replace("//","/");
            }
        }

        public ApiBuilder SetSubdomain(string subdomain)
        {
            builder.Host = string.Concat(subdomain, ".", new Uri(fullUrl).Host);
            return this;
        }

        public ApiBuilder Fragment(string fragment)
        {
            builder.Fragment = fragment;
            return this;
        }

        public bool HasSubdomain()
        {
            return builder.Uri.HostNameType == UriHostNameType.Dns
                && builder.Uri.Host.Split(".").Length > 2;
        }

        public ApiBuilder AddQueryString(string name, string value)
        {
            var qsNv = HttpUtility.ParseQueryString(builder.Query);
            qsNv[name] = string.IsNullOrEmpty(qsNv[name]) ?
                value : string.Concat(qsNv[name], ",", value);
            builder.Query = qsNv.ToString();
            return this;
        }

        public ApiBuilder QueryString(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
                builder.Query = queryString;
            return this;
        }

        public ApiBuilder Password(string password)
        {
            builder.Password = password;
            return this;
        }

        public ApiBuilder UserName(string userName)
        {
            builder.UserName = userName;
            return this;
        }

        public string GetLeftPart()
        {
            return builder.Uri.GetLeftPart(UriPartial.Path);
        }
    }
}
