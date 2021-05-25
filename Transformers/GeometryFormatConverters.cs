using System;
using System.Xml.Linq;
using NetTopologySuite.IO;
using NetTopologySuite.IO.GML2;
using WebApp.Common;

namespace WebApp.Transformers
{
    public class GeometryFormatConverters
    {
        public static string GmlToWkt(string gml, string destSrs = null)
        {
            XAttribute srsAttribute = null;

            try
            {
                var reader = new GMLReader();
                var geometry = reader.Read(gml);
                var writer = new WKTWriter();
                var res = writer.Write(geometry);

                if (!string.IsNullOrWhiteSpace(destSrs))
                {
                    var doc = XElement.Parse(gml);
                    srsAttribute = doc.Attribute(XName.Get("srsName"));

                    if (srsAttribute != null && srsAttribute.Value.ToLower() != destSrs)
                        res = CoordinateSystemTransformers.TransformWkt(res, srsAttribute.Value, destSrs);
                }

                return res;
            }
            catch (Exception e)
            {
                Logger.Error($"Не удалось преобразовать геометрию из GML в WKT. GML = {gml}, целевая СК = {destSrs}, исходная СК - {srsAttribute?.Value}", e);
                return null;
            }
        }
    }
}
