using System;
using Newtonsoft.Json;

namespace WebApp.Protocol
{
	/// <summary>
	/// Описание ошибки для ответа сервера.
	/// </summary>
	public struct Error
    {
		/// <summary>
		/// Сообщение об ошибке.
		/// </summary>
		[JsonProperty(PropertyName = "message")]
		public readonly string Message;

		/// <summary>
		/// Конструктор экземпляра.
		/// </summary>
		/// <param name="message">Сообщение об ошибке.</param>
		public Error(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
				throw new ArgumentException("Текст сообщения об ошибке не должен быть пустым.", "message");

			Message = message;
		}
	}
}
