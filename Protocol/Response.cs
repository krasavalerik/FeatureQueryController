using Newtonsoft.Json;

namespace WebApp.Protocol
{
    /// <summary>
	/// Объект, содержащий в себе данные для ответа сервера.
	/// </summary>
    public class Response
    {
        /// <summary>
		/// Обозначение успешного выполнения.
		/// </summary>
		private const string Success = "success";

        /// <summary>
		/// Обозначение ошибки.
		/// </summary>
		private const string Failure = "failure";

        /// <summary>
		/// Данные.
		/// </summary>
		[JsonProperty(PropertyName = "data")]
        public object Data { get; private set; }

        /// <summary>
        /// Статус ответа.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; private set; }

        /// <summary>
        /// Реализует создание ответа с данными.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Возвращает текущий экземпляр ответа.</returns>
        public static Response CreatePayload(object data = null)
        {
            var resp = new Response { Data = data, Status = Success };
            return resp;
        }

        /// <summary>
		/// Реализует создание ответа с описанием ошибки.
		/// </summary>
		/// <param name="message">Сообщение об ошибке.</param>
		/// <returns>Возвращает текущий экземпляр ответа.</returns>
		public static Response CreateError(string message)
        {
            var resp = new Response { Data = new Error(message), Status = Failure };
            CreatePayload(new object());
            return resp;
        }
    }
}
