using System;
using WebApp.MapServices;

namespace WebApp.Selectors
{
    public class MapProviderAliasSelector
    {
        /// <summary>
        /// Выделяет из типа слоя/карты конкретный тип провайдера
        /// </summary>
        /// <param name="layerType">Тип слоя/карты</param>
        /// <returns>Тип провайдера</returns>
        public static string GetAliasByLayerType(string layerType)
        {
            switch (layerType)
            {
                case MapProviderConstants.CACHED_LAYER:
                    return MapProviderConstants.MAPCACHE_ALIAS;
                case MapProviderConstants.WMTS:
                    return MapProviderConstants.WMTS;
                case TypeConstants.QGIS_TYPE:
                    return MapProviderConstants.QGIS_ALIAS;
                case TypeConstants.GEOSERVER_TYPE:
                    return MapProviderConstants.GEOSERVER_ALIAS;
                case TypeConstants.ROSREESTR:
                    return MapProviderConstants.ROSREESTR;
                case TypeConstants.FIRES_TYPE:
                    return MapProviderConstants.FIRES_ALIAS;
                case TypeConstants.GOOGLE_MAPS:
                    return MapProviderConstants.GOOGLE_MAPS;
                case TypeConstants.OSM:
                    return MapProviderConstants.OSM;
                case TypeConstants.ARCGIS_MAPS:
                    return MapProviderConstants.ARCGIS_ALIAS;
                case TypeConstants.GEOSERVER_WMTS:
                    return MapProviderConstants.GEOSERVER_WMTS;
                case TypeConstants.GEOSERVER_CLEAR:
                    return MapProviderConstants.GEOSERVER_CLEAR;
                default:
                    throw new InvalidOperationException($"Не удалось выбрать сервер карт по типу слоя {layerType}");
            }
        }
    }
}
