using NetTopologySuite.IO;
using GeoAPI.Geometries;

namespace WebApp.Transformers
{
    public class CoordinateSystemTransformers
    {
        /// <summary>
        /// Перепроецирование геометрии в формате WKT.
        /// </summary>
        /// <param name="wktGeometry">Строка, описывающая геометрию.</param>
        /// <param name="fromSrs">Исходная система координат.</param>
        /// <param name="toSrs">Результирующая система координат.</param>
        /// <returns>Перепроецированная геометрия в формате WKT.</returns>
        public static string TransformWkt(string wktGeometry, string fromSrs, string toSrs)
        {
            var wktReader = new WKTReader();
            var geometry = (IGeometry)wktReader.Read(wktGeometry);//TODO: не явное преобразование

            var projHelper = ProjectionHelper.GetProjectionHelper(fromSrs, toSrs);

            geometry = projHelper.ReprojectGeometry(geometry);

            var wktWriter = new WKTWriter();
            return wktWriter.Write((NetTopologySuite.Geometries.Geometry)geometry);
        }
    }
}
