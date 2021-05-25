using WebApp.MapServices.Abstract;
using WebApp.DI;
using WebApp.MapServices.Response;
using WebApp.MapServices;

namespace WebApp.Selectors
{
    /// <summary>
    /// Реализация сервиса получения провайдера географического сервера по типу слоя/карты.
    /// </summary>
    public class MapProviderSelector : IMapProviderSelector
    {
        /// <summary>
        /// Получить экземпляр службы географического сервера по типу слоя/карты
        /// </summary>
        /// <param name="layerType"> Тип слоя/карты. </param>
        /// <returns> Экземпляр географического сервера. </returns>
        public IGeoProvider Get(string layerType)
        {
            var geoProxy = new GeoProxy();
            //var geoserverSpecific = new GeoserverSpecific(); // Не нужен, его не переносить в GeoserverGeoProvider.
            var wmsParser = new /*GeoserverWfsResponseParser();*/ GeoserverWmsResponseParser();
            var wms = new GeoserverWmsService(wmsParser, geoProxy);

            var wfsParser = new GeoserverWfsResponseParser();
            var wfs = new GeoserverWfsService(wfsParser, geoProxy);

            return new GeoserverGeoProvider(wms, wfs/*, geoserverSpecific*/);
        }

        //public IGeoProvider Get(string layerType)
        //{
        //    var alias = MapProviderAliasSelector.GetAliasByLayerType(layerType);
        //    return LightInjectCore.GetService<IGeoProvider>(alias);
        //}
    }
}
