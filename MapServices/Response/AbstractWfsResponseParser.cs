using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using WebApp.MapServices.Abstract;

namespace WebApp.MapServices.Response
{
    public abstract class AbstractWfsResponseParser : IWfsResponseParser
    {
        /// <summary>
        /// Поле, использующееся для отображения в дереве.
        /// </summary>
        private const string DISPLAY_FIELD = "displayField";

        /// <summary>
        /// Поле, содержащее геометрию фич слоя.
        /// </summary>
        private const string GEOMETRY_FIELD = "geometryField";

        /// <summary>
        /// Скрытое поле.
        /// </summary>
        private const string HIDDEN_FIELD = "hiddenField";

        /// <summary>
        /// Сообщение об ошибки парсинга результата транзакции по созданию объекта.
        /// </summary>
        private const string MSG_ERROR = "Не удалось получить результат транзакции.";

        public abstract GetFeatureResponseItem[] ParseFeatures(GeoResponse response);

        public abstract DescribeFeatureInfo ParseDescribeFeature(GeoResponse response);

        public virtual WfsCapabilities[] ParseCapabilities(GeoResponse response)
        {
            using (var stream = new MemoryStream(response.Response))
            {
                var doc = new XmlDocument();
                doc.Load(stream);

                var xmlnsUrl = doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration
                    ? doc.ChildNodes[1].Attributes["xmlns"].Value
                    : doc.FirstChild.Attributes["xmlns"].Value;

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("base", xmlnsUrl);

                var featureTypes = doc.SelectNodes("//base:FeatureTypeList/base:FeatureType", nsmgr);

                var layerNames = new List<WfsCapabilities>();

                if (featureTypes == null)
                    return layerNames.ToArray();

                foreach (XmlNode fType in featureTypes)
                {
                    var nameNode = fType.SelectSingleNode("base:Name", nsmgr);
                    var titleNode = fType.SelectSingleNode("base:Title", nsmgr);
                    var abstractNode = fType.SelectSingleNode("base:Abstract", nsmgr);
                    var additionalData = abstractNode != null ? ParseAbstractNode(abstractNode.InnerText) : new List<KeyValuePair<string, string>>(0);

                    var hiddenFields = new List<string>();

                    if (nameNode == null)
                        continue;

                    var layer = new WfsCapabilities
                    {
                        Name = nameNode.InnerText,
                        Title = titleNode?.InnerText ?? nameNode.InnerText
                    };

                    foreach (var data in additionalData)
                    {
                        switch (data.Key)
                        {
                            case DISPLAY_FIELD:
                                layer.DisplayFieldName = data.Value;
                                break;
                            case GEOMETRY_FIELD:
                                layer.GeometryFieldName = data.Value;
                                break;
                            case HIDDEN_FIELD:
                                hiddenFields.Add(data.Value);
                                break;
                        }
                    }

                    layer.HiddenFields = hiddenFields;


                    layerNames.Add(layer);
                }
                return layerNames.ToArray();
            }
        }

        /// <summary>
        /// Парсинг любого узла.
        /// </summary>
        /// <param name="text"> Содержимое узла. </param>
        /// <returns> Перечисление пар имя-значение. </returns>
        private static IEnumerable<KeyValuePair<string, string>> ParseAbstractNode(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new KeyValuePair<string, string>[0];

            var items = text.Split('\n');
            var result = new List<KeyValuePair<string, string>>();

            for (var i = 0; i < items.Length; i++)
            {
                var parts = items[i].Split('=').Select(s => s.Trim()).ToArray();
                if (parts.Length != 2)
                    continue;

                result.Insert(i, new KeyValuePair<string, string>(parts[0], parts[1]));
            }

            return result;
        }
    }
}
