using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Modules
{
    public interface IGeospatialEnquirer
    {
        /// <summary>
        /// Осуществляет геозапрос для слоя по данным координатам
        /// </summary>
        /// <param name="queryLayer">Слой, для которого осуществляется запрос</param>
        /// <param name="coordinates">Координаты запроса</param>
        /// <param name="pixelToCoordinateTransform"> Параметры трансформации. </param>
        /// <param name="crs">Система кооридинат</param>
        /// <returns>Список найденных объектов для данного слоя</returns>
        IList<DtoFeature> EnquireForLayer(DtoQueryLayer queryLayer, double[] coordinates, double[] pixelToCoordinateTransform,
            string crs);
    }
}
