namespace WebApp.MapServices
{
    /// <summary>
    /// Константы для типов карт и слоев, получаемых из БД.
    /// </summary>
    public class TypeConstants
    {
        /// <summary>
        /// Обозначения типа для кэшированных слоев.
        /// </summary>
        public const string CACHED_TYPE = "cached";

        /// <summary>
        /// Тип сервера карт (Пожары).
        /// </summary>
        public const string FIRES_TYPE = "fires";

        /// <summary>
        /// Тип сервера карт.
        /// </summary>
        public const string GEOSERVER_TYPE = "geoserver";

        /// <summary>
        /// Наименование типа слоев, источник которых Google Maps.
        /// </summary>
        public const string GOOGLE_MAPS = "google_maps";

        /// <summary>
        /// Наименование типа слоев, источник которых OSM.
        /// </summary>
        public const string OSM = "osm";

        /// <summary>
        /// Тип сервера карт.
        /// </summary>
        public const string QGIS_TYPE = "qgis";

        /// <summary>
        /// Наименование типа слоев Росреестра.
        /// </summary>
        public const string ROSREESTR = "rosreestr";

        /// <summary>
        /// Наименование типа слоев, источник котороых GeoServer (WMTS).
        /// </summary>
        public const string GEOSERVER_WMTS = "geoserver_wmts";

        /// <summary>
        /// Тип слоев ArcGIS'а.
        /// </summary>
        public const string ARCGIS_MAPS = "arcgis";

        /// <summary>
        /// Тип слоев GeoServer с указанием рабочей области в URI.
        /// </summary>
        public const string GEOSERVER_CLEAR = "geoserver_clear";

        /// <summary>
        /// Внешние серверы.
        /// </summary>
        public static string[] OutsideSources => new[] { FIRES_TYPE, ROSREESTR, GOOGLE_MAPS, OSM };

        /// <summary>
        /// Серверы, у которых слой имеет фиктивный подслой.
        /// </summary>
        /// <remarks>
        /// Фиктивный подслой нужен для корректной работы со слоев в дереве слоев.
        /// </remarks>
        public static string[] SourcesWithFakeSublayer => new[] { FIRES_TYPE, CACHED_TYPE, GOOGLE_MAPS, OSM };
    }
}
