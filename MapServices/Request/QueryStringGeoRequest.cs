using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WebApp.Models;

namespace WebApp.MapServices.Request
{
    public class QueryStringGeoRequest : NameValueCollection, IGeoRequest
    {
        public string Url { get; }
        public RequestTypes RequestType { get; }
        public NetworkCredential Credentials { get; }
        public Encoding Encoding { get; set; }

        /// <inheritdoc cref="IGeoRequest.Headers"/>
        public IDictionary<string, IEnumerable<string>> Headers { get; set; }

        public QueryStringGeoRequest(string url, RequestTypes requestType, NetworkCredential credentials = null)
        {
            var uri = new Uri(url);
            var path = uri.GetLeftPart(UriPartial.Path);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            if (queryParams.Count > 0)
                Add(queryParams);

            Url = path;
            RequestType = requestType;
            Credentials = credentials;
            Encoding = Encoding.UTF8;
        }

        public string GetQuery()
        {
            var sb = new StringBuilder();
            foreach (var key in AllKeys)
            {
                var param = new[] { key, this[key] };
                sb.Append(string.Join("=", param));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"Request: {Url}; Type: {RequestType} Query params: unknown; Encoding: {Encoding}; Headers: {Headers?.Select(v => $"{v.Key}->{string.Join(",", v.Value)}")}";
        }
    }
}
