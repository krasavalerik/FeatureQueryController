using WebApp.MapServices.Abstract;

namespace WebApp.MapServices.Response
{
    public interface IWfsResponseParser
    {
        GetFeatureResponseItem[] ParseFeatures(GeoResponse response);

        /// <summary>
        /// Получение имен слоев.
        /// </summary>
        /// <param name="response"> Гео-ответ. </param>
        /// <returns> Список слоев. </returns>
        WfsCapabilities[] ParseCapabilities(GeoResponse response);

        DescribeFeatureInfo ParseDescribeFeature(GeoResponse response);
    }
}
