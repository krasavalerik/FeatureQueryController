using WebApp.MapServices.Abstract;

namespace WebApp.Selectors
{
    /// <summary>
    /// Сервис для выбора провайдера географического сервера.
    /// </summary>
    public interface IMapProviderSelector
    {
        /// <summary>
        /// Получить экземпляр провайдера географического сервера.
        /// </summary>
        /// <param name="layerType"> Тип географического сервера. </param>
        /// <returns>Экземпляр географического сервера указанного типа. </returns>
        IGeoProvider Get(string layerType);
    }
}
