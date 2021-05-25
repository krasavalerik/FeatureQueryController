using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using WebApp.MapServices.Abstract;

namespace WebApp.MapServices.Response
{
    public class GeoserverWfsResponseParser : AbstractWfsResponseParser
    {
        public override GetFeatureResponseItem[] ParseFeatures(GeoResponse response)
        {
            using (var stream = new MemoryStream(response.Response))
            {
                var featuresDoc = XDocument.Load(stream);
                var featuresCollection = featuresDoc.Root;
                if (featuresCollection == null || featuresCollection.Name.LocalName != "FeatureCollection")
                    throw new InvalidCastException($@"Ответ не содержит контейнера с объектами:{Environment.NewLine} RESPONSE:  {(response?.Response == null ? "NULL" : Encoding.UTF8.GetString(response.Response))}");

                var elements = featuresCollection.Elements(XName.Get("featureMember",
                    featuresCollection.GetNamespaceOfPrefix("gml").NamespaceName)).ToList();

                if (!elements.Any())
                {
                    elements = featuresCollection.Elements(XName.Get("featureMembers",
                        featuresCollection.GetNamespaceOfPrefix("gml").NamespaceName)).ToList();
                }

                var members = elements.SelectMany(m => m.Elements()).Where(m => m != null);

                return members.Select(ParseFeature).ToArray();
            }
        }

        public override DescribeFeatureInfo ParseDescribeFeature(GeoResponse response)
        {
            var result = new DescribeFeatureInfo();
            using (var stream = new MemoryStream(response.Response))
            {
                var featureInfoDoc = XDocument.Load(stream);

                var xsd = featureInfoDoc.Root.GetNamespaceOfPrefix("xsd");

                var layers = featureInfoDoc.Root.Elements(xsd + "complexType");

                foreach (var layerTag in layers)
                {
                    var attrs = new List<DescribeFeatureInfoAttribute>();

                    var layerName = layerTag.Attribute("name").Value.Replace("Type", string.Empty);

                    var attributes = layerTag.Element(xsd + "complexContent")
                        .Element(xsd + "extension")
                        .Element(xsd + "sequence")
                        .Elements(xsd + "element");

                    foreach (var attributeTag in attributes)
                    {
                        var nameAttr = attributeTag.Attribute("name");
                        var typeAttr = attributeTag.Attribute("type");

                        var featureAttribute = new DescribeFeatureInfoAttribute
                        {
                            Name = nameAttr?.Value,
                            Type = typeAttr?.Value
                        };
                        attrs.Add(featureAttribute);
                    }

                    result.Add(layerName, attrs);
                }
            }

            return result;
        }

        private GetFeatureResponseItem ParseFeature(XElement featureNode)
        {
            var res = new GetFeatureResponseItem()
            {
                Fid = featureNode.Attribute(XName.Get("fid"))?.Value
            };
            //var featureNode = featureMember.Elements().FirstOrDefault();

            res.TypeName = $"{featureNode.GetPrefixOfNamespace(featureNode.Name.Namespace)}:{featureNode.Name.LocalName}";

            foreach (var element in featureNode.Elements())
            {
                var name = element.Name.LocalName;
                var value = element.HasElements ? element.Elements().First().ToString() : element.Value;
                res.Attributes[name] = value;
            }

            return res;
        }
    }
}
