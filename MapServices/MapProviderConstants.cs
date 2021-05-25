using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.MapServices
{
    public static class MapProviderConstants
    {
        public const string GEOSERVER_ALIAS = "geo";
        public const string QGIS_ALIAS = "qgis";
        public const string MAPCACHE_ALIAS = "mc";

        /// <summary>
        /// Наименование провайдера для слоев Росреестра.
        /// </summary>
        public const string ROSREESTR = "rosreestr";

        /// <summary>
        /// Наименование типа слоев, источник которых Fires.
        /// </summary>
        public const string FIRES_ALIAS = "fires";

        /// <summary>
        /// Наименование типа соев для ArcGIS.
        /// </summary>
        public const string ARCGIS_ALIAS = "arcgis";

        /// <summary>
        /// Наименование типа слоев, источник которых Google Maps.
        /// </summary>
        public const string GOOGLE_MAPS = "google_maps";

        /// <summary>
        /// Наименование типа слоев, источник которых OpenStreetMaps.
        /// </summary>
        public const string OSM = "osm";

        /// <summary>
        /// Имя типа кэшированного слоя (в таблице layers)
        /// </summary>
        public const string CACHED_LAYER = "cached";

        /// <summary>
        /// Наименование типа слоев, источник которых GeoServer (WMTS).
        /// </summary>
        public const string GEOSERVER_WMTS = "geoserver_wmts";

        public const string WMTS = "wmts";

        /// <summary>
        /// Тип слоев GeoServer с указанием рабочей области в URI.
        /// </summary>
        public static string GEOSERVER_CLEAR = "geoserver_clear";
    }
}
