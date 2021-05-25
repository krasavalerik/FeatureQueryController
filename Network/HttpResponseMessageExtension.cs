using System.Net.Http;

namespace WebApp.Network
{
    /// <summary>
    /// Класс-расширение <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class HttpResponseMessageExtension
    {
        /// <summary>
        /// Получить содержание сообщение в виде массива <see cref="byte"/>.
        /// </summary>
        /// <param name="message"> Сообщение. </param>
        /// <returns> Массив <see cref="byte"/>. </returns>
        public static byte[] GetBytesContent(this HttpResponseMessage message)
        {
            return message.IsSuccessStatusCode
                ? message.Content.ReadAsByteArrayAsync().Result
                : new byte[0];
        }

        /// <summary>
        /// Получить тип содержимого <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="message"> Сообщение. </param>
        /// <returns> Тип содержимого. </returns>
        public static string GetContentType(this HttpResponseMessage message)
        {
            return message?.Content?.Headers?.ContentType?.MediaType ?? string.Empty;
        }
    }
}
