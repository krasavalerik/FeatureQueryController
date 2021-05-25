using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using WebApp.Settings;
using WebApp.Models;
using Newtonsoft.Json;
using WebApp.Modules;

namespace WebApp.Services
{
    /// <summary>
    /// Сервис по работе с запросами по feature. 
    /// </summary>
    public class FeatureQueryService
    {
        /// <summary>
        /// Экземпляр сервиса географических запросов объектов.
        /// </summary>
        private readonly IGeospatialEnquirer _enquirer;

        /// <summary>
        /// Полный формат даты.
        /// </summary>
        private const string FULL_DATE_FORMAT = "dd.MM.yyyy H:mm:ss";

        /// <summary>
        /// Сокращенный формат даты.
        /// </summary>
        private const string SHORT_DATE_FORMAT = "dd.MM.yyyy";

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="enquirer"> Сервис географических запросов объектов. </param>
        public FeatureQueryService(IGeospatialEnquirer enquirer)
        {
            _enquirer = enquirer;
        }

        /// <summary>
        /// Найти объекты по точке.
        /// </summary>
        /// <param name="query"> Данные для поиска объекта. </param>
        /// <returns> Возвращает найденные объекты. </returns>
        public IList<DtoFeature> SearchFeaturesByPoint(DtoFeaturesGeoQuery query)
        {
            var coordinates = JsonConvert.DeserializeObject<double[]>(query.Geometry);
            var pixelToCoordinateTransform = new[]
            {
                query.PixelToCoordinateTransform[0],
                query.PixelToCoordinateTransform[3]
            };

            var features = query.Layers.Aggregate(
                new List<DtoFeature>(),
                (acc, queryLayer) =>
                {
                    var foundFeatures = _enquirer.EnquireForLayer(
                        queryLayer,
                        coordinates,
                        pixelToCoordinateTransform,
                        query.Crs);
                    acc.AddRange(foundFeatures.Reverse());
                    return acc;
                });

            var orderedFeatures = features.OrderBy(it => it.LayerOrder).Reverse().ToList();
            //NormalizeGlobalIdAttribute(orderedFeatures);

            return orderedFeatures;
        }

        /// <summary>
        /// Преобразовать даты в dd.MM.yyyy.
        /// </summary>
        /// <param name="features"> Объекты. </param>
        /// <remarks> Ленивое исполнение. </remarks>
        public IEnumerable<DtoFeature> ConvertDateTimeAttribute(IEnumerable<DtoFeature> features) => features.Select(feature =>
        {
            foreach (var key in feature.Attributes.Keys.ToArray())
            {
                var isDate = DateTime.TryParse(feature.Attributes[key], out var date);

                if (!isDate)
                    continue;

                var isDouble = double.TryParse(feature.Attributes[key].Replace(',', '.'), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out _);

                if (isDouble)
                    continue;

                var format = date.Hour == 0 && date.Minute == 0 && date.Second == 0
                    ? SHORT_DATE_FORMAT : FULL_DATE_FORMAT;
                feature.Attributes[key] = date.ToString(format);
            }

            return feature;
        });

        /// <summary>
        /// Нормализует наименование атрибута глобального ID к указанному в настройках (globalIdFieldName), если таковой присутствует.
        /// </summary>
        public void NormalizeGlobalIdAttribute(IEnumerable<DtoFeature> dtoFeatures)
        {
            var globalIdAttributeName = SettingsReader.Instance.GisConfigValues.GlobalIdFieldName;
            var globalIdFieldAliases = SettingsReader.Instance.GisConfigValues.GlobalIdFieldAliases;

            foreach (var dtoFeature in dtoFeatures)
            {
                var attrNames = dtoFeature.Attributes.Keys.ToList();
                var globalIdName = attrNames.FirstOrDefault(x =>
                    x.Equals(globalIdAttributeName, StringComparison.InvariantCultureIgnoreCase) ||
                    globalIdFieldAliases.Contains(x));

                if (globalIdName != null &&
                    !globalIdName.Equals(globalIdAttributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var guid = dtoFeature.Attributes[globalIdName];

                    dtoFeature.Attributes.Remove(globalIdName);
                    dtoFeature.Attributes.Add(globalIdAttributeName, guid);
                }
            }
        }
    }
}
