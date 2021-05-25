using Newtonsoft.Json;

namespace WebApp.Models
{
	/// <summary>
	/// DTO для запроса по слою
	/// </summary>
	public sealed class DtoQueryLayer
    {
		/// <summary>
		/// ID слоя
		/// </summary>
		[JsonProperty("id")]
		public long Id { get; set; }

		/// <summary>
		/// Список подслоев
		/// </summary>
		[JsonProperty("sublayers")]
		public string[] Sublayers { get; set; }
	}
}
