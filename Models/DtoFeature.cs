using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApp.Models
{
    /// <summary>
    /// Запрашиваемый объект.
    /// </summary>
    public class DtoFeature
    {
        /// <summary>
        /// Компаратор для сравнения сущностей.
        /// </summary>
        //public static readonly DtoFeatureInfoComparer DtoFeatureInfoComparer = new DtoFeatureInfoComparer();

        public DtoFeature()
        {
            Attributes = new Dictionary<string, string>();
        }

        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public string Fid { get; set; }

        /// <summary>
        /// Id объекта.
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// Id подслоя.
        /// </summary>
        [JsonProperty("layerId")]
        public long LayerId { get; set; }

        /// <summary>
        /// Наименование слоя
        /// </summary>
        [JsonProperty("layerName")]
        public string LayerName { get; set; }

        /// <summary>
        /// Название подслоя.
        /// </summary>
        [JsonProperty("sublayerName")]
        public string SublayerName { get; set; }

        /// <summary>
        /// Наименование сервиса.
        /// </summary>
        [JsonProperty("sublayerTypeName")]
        public string SublayerTypeName { get; set; }

        /// <summary>
        /// Название поля, использующегося для отображения в дереве, устанавливается на сервере ГИС.
        /// </summary>
        [JsonProperty("displayFieldName")]
        public string DisplayFieldName { get; set; }

        /// <summary>
        /// Список скрытых полей.
        /// </summary>
        [JsonProperty("hiddenFields")]
        public List<string> HiddenFields { get; set; }

        /// <summary>
        /// Значение поля Display Field Name.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Набор атрибутов.
        /// </summary>
        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Геометрия объекта в формате WKT. 
        /// </summary>
        [JsonProperty("geometry")]
        public string Geometry { get; set; }

        /// <summary>
        /// Площадь объекта.
        /// </summary>
        [JsonProperty("area")]
        public double Area { get; set; }

        /// <summary>
        /// Порядок слоев.
        /// </summary>
        [JsonIgnore]
        public long LayerOrder { get; set; }
    }
}
