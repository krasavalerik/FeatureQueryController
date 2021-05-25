using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WebApp.Models
{
    /// <summary>
	/// Модель для передачи найденых объектов и служебных атрибуты.
	/// </summary>
    public class DtoSearchResult
    {
        /// <summary>
		/// Найденые объекты.
		/// </summary>
		[JsonProperty("foundObjects")]
        public IList<DtoFeature> FoundObjects { get; set; }

        /// <summary>
		/// Служебные атрибуты.
		/// </summary>
		[JsonProperty("systemFieldsNames")]
        public List<string> SystemFieldsNames { get; set; }

        /// <summary>
        /// Ошибки во время поиска.
        /// </summary>
        [JsonProperty("errors")]
        public IList<string> Errors { get; set; }

        /// <summary>
		/// Конструктор без параметров.
		/// </summary>
		//public DtoSearchResult()
  //      {
  //          var sr = SettingsReader.Instance;
  //          SystemFieldsNames = sr.GisConfigValues.SystemFieldNames.ToList();
  //      }

        public DtoSearchResult() { }

        /// <summary>
        /// Конструктор с принимаемым списком найденных объектов.
        /// </summary>
        /// <param name="foundObjects">Список найденных объектов.</param>
        /// <param name="errors">Список ошибок </param>
        public DtoSearchResult(IList<DtoFeature> foundObjects, IList<string> errors = null) : this()
        {
            FoundObjects = foundObjects;
            Errors = errors?.Distinct().ToList() ?? new List<string>(0);
        }
    }
}
