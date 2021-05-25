using System;
using System.Collections.Generic;
using WebApp.Models;
using WebApp.Repositorys;
using WebApp.Selectors;
using WebApp.Common;
using System.Net;
using WebApp.Entitys;

namespace WebApp.Modules
{
    /// <summary>
    /// Сервис географических запросов объектов.
    /// </summary>
    public class GeospatialEnquirer : IGeospatialEnquirer
    {
        private readonly /*LayerRepository*/GenericRepository<Layer> _layerRepository;

        /// <summary>
        /// Селектор провайдера карт
        /// </summary>
        private readonly IMapProviderSelector _mapProviderSelector;

        public GeospatialEnquirer(IMapProviderSelector mapProviderSelector = null, /*LayerRepository*/GenericRepository<Layer> layerRepository = null)
        {
            _mapProviderSelector = mapProviderSelector;// ?? LightInjectCore.GetService<IMapProviderSelector>();
            _layerRepository = layerRepository;// ?? new LayerRepository();
        }

        public IList<DtoFeature> EnquireForLayer(DtoQueryLayer queryLayer, double[] coordinates,
            double[] pixelToCoordinateTransform, string crs)
        {
            var layer = _layerRepository.Get(queryLayer.Id);

            if (layer == null) throw new ArgumentException(/*"Не найден слой с ID = {0}".FormatWith(queryLayer.Id)*/);

            try
            {
                var geoProvider = _mapProviderSelector.Get(layer.Type);
                //TODO: рефлизовать FindFeaturesByCoordinate()
                var features = geoProvider.FindFeaturesByCoordinate(
                    layer,
                    queryLayer.Sublayers,
                    coordinates,
                    pixelToCoordinateTransform,
                    crs);

                foreach (var dtoFeature in features)
                {
                    dtoFeature.LayerOrder = layer.LayerOrder;
                }

                return features;
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError)
                    throw;

                var response = ex.Response as HttpWebResponse;
                if (response == null)
                    throw;

                Logger.Error("Мап сервер вернул протокольную ошибку"/*, код {0}".FormatWith((int)response.StatusCode)*/);
                return new DtoFeature[0];
            }
        }
    }
}
