using System.Net.Http;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;

namespace WebApp.Network
{
    /// <summary>
    /// Кастомный <see cref="WebClient"/> с возможностью переиспользования и исполнения параллельных запросов. Основан на <see cref="HttpClient"/>.
    /// </summary>
    public class CustomWebClient
    {
        /// <summary>
        /// Клиент.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public CustomWebClient()
        {
            var handler = new HttpClientHandler { UseDefaultCredentials = true };
            _client = new HttpClient(handler);
        }

        /// <summary>
        /// Загружает ресурс, как массив System.Byte из заданного URI.
        /// </summary>
        /// <param name="address"> URI, с которого будут загружены данные. </param>
        /// <param name="contentType"> Тип содержания. </param>
        /// <param name="headers"> Заголовки. </param>
        /// <returns> Массив <see cref="byte"/>, содержащий загруженный ресурс. </returns>
        public byte[] DownloadData(string address, out string contentType,
            IDictionary<string, IEnumerable<string>> headers = null)
        {
            UpdateHeaders(headers);
            var response = _client.GetAsync(address).Result;

            contentType = response.GetContentType();

            return response.GetBytesContent();
        }

        /// <summary>
        /// Передает указанную коллекцию "имя-значение" указанному ресурсу, указанному с помощью URI, используя указанный метод.
        /// </summary>
        /// <param name="address"> URI ресурса, которому передается коллекция. </param>
        /// <param name="collection"> Коллекция <see cref="NameValueCollection"/>, передаваемая ресурсу. </param>
        /// <param name="contentType"> Тип содержания. </param>
        /// <param name="headers"> Заголовки. </param>
        /// <returns> Массив значений типа <see cref="byte"/>, содержащий основной текст ответа ресурса. </returns>
        public byte[] UploadValues(string address, NameValueCollection collection, out string contentType,
            IDictionary<string, IEnumerable<string>> headers = null)
        {
            UpdateHeaders(headers);

            var urlContent = EncodeContent(collection);
            var response = _client.PostAsync(address, urlContent).GetAwaiter().GetResult();
            contentType = response.GetContentType();

            return response.GetBytesContent();
        }

        /// <summary>
        /// Передает буфер данных ресурсу, заданному с помощью URI.
        /// </summary>
        /// <param name="address"> URI ресурса, которому передаются данные. </param>
        /// <param name="data"> Буфер данных, передаваемый ресурсу. </param>
        /// <param name="contentType"> Тип содержания. </param>
        /// <param name="headers"> Заголовки. </param>
        /// <returns> Массив значений типа <see cref="byte"/>, содержащий основной текст ответа ресурса. </returns>
        public byte[] UploadData(string address, byte[] data, out string contentType,
            IDictionary<string, IEnumerable<string>> headers = null)
        {
            UpdateHeaders(headers);
            var content = new ByteArrayContent(data);
            var response = _client.PostAsync(address, content).Result;

            contentType = response.GetContentType();

            return response.GetBytesContent();
        }

        /// <summary>
        /// Обновить заголовки.
        /// </summary>
        /// <param name="headers"> Заголовки. </param>
        /// TODO: модификация объекта в многопоточности - изменить.
        private void UpdateHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            _client.DefaultRequestHeaders.Clear();

            if (headers == null)
                return;

            foreach (var (key, value) in headers)
            {
                _client.DefaultRequestHeaders.Add(key, value);
            }
        }

        /// <summary>
        /// Закодировать содержимое коллекции.
        /// </summary>
        /// <param name="collection"> Коллекция. </param>
        /// <returns> Возвращает закодированные данные. </returns>
        /// <remarks> Кодирует коллекцию для отправки методом POST через HTTP client. </remarks>
        private static StringContent EncodeContent(NameValueCollection collection)
        {
            var items = collection.AllKeys
                .Select(key =>
                {
                    var urlParam = WebUtility.UrlEncode(key);
                    var paramValue = WebUtility.UrlEncode(collection.Get(key));
                    return $"{urlParam}={paramValue}";
                });
            var urlContent = new StringContent(
                string.Join("&", items),
                null,
                MediaTypeNames.Application.X_WWW_FORM_URL_ENCODED);

            return urlContent;
        }
    }
}
