using System;
using System.Linq;
using System.Web;

namespace WebApp.MapServices
{
    public static class UrlExtensions
    {
        public static string GetHostAndPath(this Uri url)
        {
            var ub = new UriBuilder(url.Scheme, url.Host, url.Port, url.LocalPath);
            return ub.ToString();
        }

        /// <summary>
        /// Получить наименования слоя из URI.
        /// </summary>
        /// <param name="uri"> URI. </param>
        /// <returns> Наименование слоя. </returns>
        public static string GetLayerLink(this Uri uri)
        {
            var query = uri.Query;
            var queryParams = HttpUtility.ParseQueryString(query);

            return queryParams[queryParams.AllKeys.FirstOrDefault()] ?? String.Empty;
        }
    }
}
