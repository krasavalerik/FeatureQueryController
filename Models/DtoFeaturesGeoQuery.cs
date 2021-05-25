using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApp.Models
{
    /// <summary>
    /// Дто объекта для запроса.
    /// </summary>
    public class DtoFeaturesGeoQuery
    {
        [JsonProperty("geometry")]
        public string Geometry { get; set; }

        [JsonProperty("pixelToCoordinate")]
        public double[] PixelToCoordinateTransform { get; set; }

        [JsonProperty("layers")]
        public IList<DtoQueryLayer> Layers { get; set; }

        [JsonProperty("crs")]
        public string Crs { get; set; }

        [JsonProperty("extent")]
        public string Extent { get; set; }
    }
}
