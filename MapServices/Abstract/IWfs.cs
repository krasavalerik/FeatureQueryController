using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.MapServices.Abstract
{
    public interface IWfs
    {
        /// <summary>
        /// Получить объект.
        /// </summary>
        /// <param name="serverUrl"> Ссылка на сервис данных. </param>
        /// <param name="layerLink"> Ссылка на слой. </param>
        /// <param name="typeNames"> Наименования слоев. </param>
        /// <param name="bBox"> Описание экстента. </param>
        /// <param name="srs"> Описание СК. </param>
        /// <param name="geoAttributeName"> Наименование атрибута, содержащего геометрию. </param>
        /// <returns> Ответ по объектам от сервиса данных. </returns>
        GetFeatureResponseItem[] GetFeature(
            string serverUrl,
            string layerLink,
            IEnumerable<string> typeNames,
            BoundingBox bBox,
            string srs,
            string geoAttributeName = "");

        /// <summary>
        /// Получение списка слоев.
        /// </summary>
        /// <param name="url"> Url сервера. </param>
        /// <param name="layerLink"> Наименование проекта/неймспейса. </param>
        /// <returns> Wfs-capabilities. </returns>
        WfsCapabilities[] GetCapabilities(string url, string layerLink);

        /// <summary>
        /// Запрос DescribeFeatureType.
        /// </summary>
        /// <param name="serverUrl"> Url сервера. </param>
        /// <param name="layerNames"> Название слоёв, для которых необходимо получить информацию</param>
        /// <returns></returns>
        DescribeFeatureInfo DescribeFeatureType(string serverUrl, IEnumerable<string> layerNames);
    }
}
