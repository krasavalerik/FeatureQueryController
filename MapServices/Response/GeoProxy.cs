using System;
using System.Text;
using System.Collections.Specialized;
using WebApp.MapServices.Request;
using WebApp.Network;

namespace WebApp.MapServices.Response
{
    public class GeoProxy : IGeoProxy
    {
        /// <summary>
        /// Клиент для обмена данными, использующий URI.
        /// </summary>
        private static readonly CustomWebClient Client = new CustomWebClient();

        public GeoResponse ProxyRequest(IGeoRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), @"Параметр request не должен быть null.");

            byte[] data;
            string contentType;

            switch (request.RequestType)
            {
                case RequestTypes.Get:
                    data = Client.DownloadData(CreateGetRequest(request), out contentType, request.Headers);
                    break;
                case RequestTypes.Post:
                    if (request is NameValueCollection col)
                    {
                        data = Client.UploadValues(request.Url, col, out contentType, request.Headers);
                    }
                    else
                    {
                        var encoding = request.Encoding ?? Encoding.UTF8;
                        data = Client.UploadData(request.Url, encoding.GetBytes(request.GetQuery()), out contentType,
                            request.Headers);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Передан неизвестный тип запроса");
            }

            var response = new GeoResponse
            {
                ContentType = contentType,
                Response = data
            };

            return response;
        }

        private static string CreateGetRequest(IGeoRequest request)
        {
            var urlBuilder = new UriBuilder(request.Url) { Query = request.GetQuery().Replace("%", "%25") };
            return urlBuilder.Uri.ToString();
        }
    }
}
