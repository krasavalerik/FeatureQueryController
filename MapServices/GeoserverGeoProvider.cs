using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;

using WebApp.Common;
using WebApp.Models;
using WebApp.Entitys;
using WebApp.MapServices.Abstract;
using WebApp.Transformers;
using NLogger = NLog.Logger;


namespace WebApp.MapServices
{
    /// <summary>
    /// Реалиация провайдера геосервера дял GeoServer.
    /// </summary>
    public class GeoserverGeoProvider : IGeoProvider //extends AbstractGeoProvider : IGeoProvider
    {
        /// <summary>
        /// Размер квадрата поиска по координатам на карте в пикселах
        /// </summary>
        private const int BBOX_PIXEL_SIZE = 15;

        /// <summary>
        /// Текущий интерфейс взаимодействия с Web Feature Service.
        /// </summary>
        protected IWfs Wfs { get; } //TODO: ABSTRACT

        /// <summary>
        /// Текущий интерфейс взаимодействия с Web Map Service.
        /// </summary>
        protected IWms Wms { get; } //TODO: ABSTRACT

        public GeoserverGeoProvider(IWms wms, IWfs wfs) //TODO: Abstract
        {
            Wms = wms;
            Wfs = wfs;
        }

        public /*override*/ DtoFeature[] FindFeaturesByCoordinate(Layer layer, string[] sublayers,
            double[] coordinates, double[] pixelToCoordinateTransform, string outputSrs)
        {
            if (sublayers.Length == 0)
                return new DtoFeature[0];

            var bBox = CalculateBbox(coordinates, pixelToCoordinateTransform, outputSrs);

            return FindFeaturesInsideArea(layer, sublayers, bBox, outputSrs);
        }

        public /*override*/ DtoFeature[] FindFeaturesInsideArea(Layer layer, string[] sublayers, BoundingBox bbox,
            string outputSrs)
        {
            if (sublayers.Length == 0)
                return new DtoFeature[0];

            var baseUrl = new Uri(layer.Url);
            var (serverUrl, layerLink) = GetQueryInfo(baseUrl);

            var wfsLayersDict = GetWfsCapabilitiesDictionary(serverUrl, layerLink);
            var uniqueSublayers = sublayers.Distinct();

            // Выбрать только нужные подслои.
            var needWfsInfo =
                uniqueSublayers
                    .Where(sublayer => wfsLayersDict.ContainsKey(sublayer))
                    .ToDictionary(sublayer => sublayer, sublayer => wfsLayersDict[sublayer]);

            // Собрать информацию о слоях, сгруппированных по наименованиям атрибута, содержащего геоданные.
            // Ключ - наименование атрибута, значение - наименования подслоев.
            var geoLayerInfo = new Dictionary<string, ICollection<string>>();
            foreach (var wfsItem in needWfsInfo)
            {
                var key = wfsItem.Key;
                var geoName = wfsItem.Value.GeometryFieldName;

                if (geoName is null)
                {
                    Logger.Warn($"Не удалось получить геометрию из слоя, проверьте аннотацию слоя {layer.Name}.");
                    continue;
                }

                if (geoLayerInfo.ContainsKey(geoName))
                {
                    geoLayerInfo[geoName].Add(key);
                }
                else
                {
                    geoLayerInfo.Add(geoName, new List<string> { key });
                }
            }

            var foundWfsFeatures = geoLayerInfo.SelectMany(infoItem =>
            {
                var sublayersName = infoItem.Value;
                var geoAttributeName = infoItem.Key;

                var features = Wfs.GetFeature(serverUrl, layerLink, sublayersName, bbox, outputSrs, geoAttributeName); //TODO: реализовать getFeature()

                return AddMissingAttributes(features, serverUrl, sublayersName);
            });

            return ConvertFoundedFeatures(foundWfsFeatures, wfsLayersDict, layer);
        }

        protected virtual (string serverUrl, string layerLink) GetQueryInfo(Uri uri) => (uri.GetHostAndPath(), GetLayerLink(uri));

        protected /*override*/ string GetLayerLink(Uri url)
        {
            var query = url.Query;
            var queryParams = HttpUtility.ParseQueryString(query); //TODO: реализация
            return queryParams["namespace"];
        }

        protected IDictionary<string, WfsCapabilities> GetWfsCapabilitiesDictionary(string serverUrl, string layerLink)
        {
            var wfsCapabilities = Wfs.GetCapabilities(serverUrl, layerLink); //TODO: GetCapabilities() реализовать
            return wfsCapabilities.ToDictionary(c => c.Name);
        }

        protected DtoFeature[] ConvertFoundedFeatures(IEnumerable<GetFeatureResponseItem> foundFeatures,
            IDictionary<string, WfsCapabilities> wfsLayersDict, Layer layer, bool titleAsSublayerName = true, NLogger logger = null)
        {
            var features = new List<DtoFeature>();

            foreach (var wfsFeature in foundFeatures)
            {
                var dto = new DtoFeature
                {
                    Fid = wfsFeature.Fid,
                    Attributes = wfsFeature.Attributes,
                    LayerId = layer?.Id ?? 0,
                    LayerName = layer?.Name,
                    LayerOrder = layer?.LayerOrder ?? 0
                };

                features.Add(dto);

                if (!wfsLayersDict.TryGetValue(wfsFeature.TypeName, out var wfsCapability))
                    continue;

                dto.SublayerName = titleAsSublayerName ? wfsCapability.Title : wfsFeature.TypeName;
                dto.SublayerTypeName = wfsFeature.TypeName;

                if (!string.IsNullOrWhiteSpace(wfsCapability.DisplayFieldName))
                    dto.DisplayFieldName = wfsCapability.DisplayFieldName;

                if (!string.IsNullOrWhiteSpace(dto.DisplayFieldName) &&
                    dto.Attributes.TryGetValue(dto.DisplayFieldName, out var attributeValue))
                {
                    dto.Value = attributeValue;
                }

                if (!string.IsNullOrWhiteSpace(wfsCapability.GeometryFieldName)
                    && wfsFeature.Attributes.ContainsKey(wfsCapability.GeometryFieldName))
                {
                    dto.Geometry = GeometryFormatConverters
                        .GmlToWkt(wfsFeature.Attributes[wfsCapability.GeometryFieldName]);

                    if (string.IsNullOrWhiteSpace(dto.Geometry))
                    {
                        logger?.Warn($@"Не удалось десериализовать геометрию - ""{wfsFeature.Attributes[wfsCapability.GeometryFieldName]}"" у объекта с id = ""{dto.Id}"", id слоя = ""{dto.LayerId}"", наименование подслоя = ""{dto.SublayerName}"".");
                    }

                    wfsFeature.Attributes.Remove(wfsCapability.GeometryFieldName);
                }

                dto.Attributes = RemoveHiddenAttributes(wfsFeature.Attributes, wfsCapability.HiddenFields);
            }

            return features.ToArray();
        }

        /// <summary>Расчитывает географическую область для запроса по координатам.</summary>
        /// <param name="coordinates">Координаты для запроса.</param>
        /// <returns>The <see cref="IEnumerable"/>Область в виде массива координат в порядке: xMin, yMin, xMax, yMax.</returns>
        private static BoundingBox CalculateBbox(double[] coordinates, double[] pixelToCoordinateTransform, string srs)
        {
            var deltaX = (double)BBOX_PIXEL_SIZE / 2 * pixelToCoordinateTransform[0];
            var deltaY = (double)BBOX_PIXEL_SIZE / 2 * pixelToCoordinateTransform[1];
            var x = coordinates[0];
            var y = coordinates[1];
            var xCoords = new[] { x - deltaX, x + deltaX };
            var yCoords = new[] { y - deltaY, y + deltaY };
            var xMin = xCoords.Min();
            var xMax = xCoords.Max();
            var yMin = yCoords.Min();
            var yMax = yCoords.Max();

            return new BoundingBox(xMin, yMin, xMax, yMax, srs);
        }

        /// <summary>
        /// Добавление всех стобцов из таблицы в атрибуты.
        /// </summary>
        /// <param name="foundFeatures"> Фичи. </param>
        /// <param name="serverUrl"> Адрес сервера. </param>
        /// <param name="sublayersName"> Название слоев. </param>
        /// <returns></returns>
        private IEnumerable<GetFeatureResponseItem> AddMissingAttributes(IEnumerable<GetFeatureResponseItem> foundFeatures, string serverUrl, 
            ICollection<string> sublayersName)
        {
            var featuresInfo = Wfs.DescribeFeatureType(serverUrl, sublayersName);
            var features = foundFeatures.ToList();

            foreach (var feature in features)
            {
                var name = feature.TypeName.Split(':')[1];

                if (!featuresInfo.ContainsKey(name))
                    continue;

                foreach (var attribute in featuresInfo[name])
                {
                    if (!feature.Attributes.ContainsKey(attribute.Name))
                    {
                        feature.Attributes[attribute.Name] = string.Empty;
                    }
                }
            }

            return features;
        }

        /// <summary>
        /// Удалить атрибуты, отмеченные как скрытые.
        /// </summary>
        /// <param name="attributes"> Все атрибуты. </param>
        /// <param name="hiddenFields"> Наименования скрытых атрибутов. </param>
        /// <returns> Обновленный словарь. </returns>
        private static Dictionary<string, string> RemoveHiddenAttributes(Dictionary<string, string> attributes, IEnumerable<string> hiddenFields)
        {
            foreach (var hiddenField in hiddenFields)
            {
                attributes.Remove(hiddenField);
            }

            return attributes;
        }
    }
}
