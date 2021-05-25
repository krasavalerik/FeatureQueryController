using Newtonsoft.Json;

namespace WebApp.Entitys
{
    /// <summary>
	/// Слой ГИСП3.
	/// </summary>
    public class Layer
    {
		/// <summary>
		/// Идентификатор сущности.
		/// </summary>
		//[DisplayName(@"ИД")] //TODO переделать на GUID
		public virtual long Id { get; set; } //:BaseEntity

		///// <summary>
		///// Карта, с которой связан данный слой.
		///// </summary>
		//[JsonIgnore]
		//public virtual Map Map { get; set; }

		/// <summary>
		/// ID карты, с которой связан данный слой.
		/// </summary>
		[JsonProperty("mapId")]
		public virtual long MapId { get; set; }

		/// <summary>
		/// Наименование слоя.
		/// </summary>
		[JsonProperty("name")]
		public /*override*/ string Name { get; set; }

		/// <summary>
		/// URL источника данных слоя.
		/// </summary>
		[JsonProperty("url")]
		public virtual string Url { get; set; }

		/// <summary>
		/// Тип слоя.
		/// </summary>
		[JsonProperty("type")]
		public virtual string Type { get; set; }

		/// <summary>
		/// Флаг активности данного слоя.
		/// </summary>
		[JsonProperty("isActive")]
		public virtual bool IsActive { get; set; }

		/// <summary>
		/// Флаг, развернут ли данный слой.
		/// </summary>
		[JsonProperty("isExpanded")]
		public virtual bool IsExpanded { get; set; }

		/// <summary>
		/// Непрозрачность слоя по-умолчанию.
		/// </summary>
		[JsonProperty("defaultOpacity")]
		public virtual byte DefaultOpacity { get; set; }

		/// <summary>
		/// Порядок слоя по-умолчанию.
		/// </summary>
		[JsonProperty("layerOrder")]
		public virtual long LayerOrder { get; set; }

		///// <summary>
		///// Список подслоев данного слоя.
		///// </summary>
		//[JsonProperty("sublayers")]
		//public virtual Sublayer[] Sublayers { get; set; }

		/// <summary>
		/// Флаг, является ли данный слой базовой картой.
		/// </summary>
		[JsonIgnore]
		public virtual bool IsBaseMap { get; set; }

		/// <summary>
		/// Флаг, является ли данный слой удаленным.
		/// </summary>
		[JsonIgnore]
		public virtual bool IsDeleted { get; set; }

		/// <summary>
		/// Флаг привязки.
		/// </summary>
		[JsonProperty("isSnappable")]
		public virtual bool IsSnappable { get; set; }

		///// <summary>
		///// Доступные масштабы.
		///// </summary>
		//[JsonIgnore]
		//public virtual IList<LayerLodUnavailability> LayerLodUnavailabilities { get; set; }

		/// <summary>
		/// Флаг недоступности слоя для поиска.
		/// </summary>
		[JsonProperty("isUnsearchable")]
		public virtual bool? IsUnsearchable { get; set; }

		/// <summary>
		/// Наименование слоя.
		/// </summary>
		[JsonProperty("groupLayer")]
		public virtual string GroupLayer { get; set; }
	}
}
