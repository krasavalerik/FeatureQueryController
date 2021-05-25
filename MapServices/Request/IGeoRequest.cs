using System.Net;
using System.Text;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.MapServices.Request
{
    public interface IGeoRequest
    {
        string Url { get; }

        string GetQuery();

        RequestTypes RequestType { get; }

        NetworkCredential Credentials { get; }

        Encoding Encoding { get; }

        /// <summary>
        /// Заголовки запроса.
        /// </summary>
        IDictionary<string, IEnumerable<string>> Headers { get; set; }
    }
}
