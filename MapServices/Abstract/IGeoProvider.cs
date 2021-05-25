using WebApp.Entitys;
using WebApp.Models;

namespace WebApp.MapServices.Abstract
{
    /// <summary>
    /// Интерфейс провайдера для работы с геоинформационным сервером.
    /// </summary>
    public interface IGeoProvider
    {
        DtoFeature[] FindFeaturesByCoordinate(Layer layer,
            string[] sublayers,
            double[] coordinates,
            double[] pixelToCoordinateTransform,
            string crs);
    }
}
