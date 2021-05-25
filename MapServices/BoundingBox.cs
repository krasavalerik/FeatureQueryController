using System;
using System.Globalization;

namespace WebApp.MapServices
{
    public class BoundingBox
    {
        public double XMin { get; }
        public double YMin { get; }
        public double XMax { get; }
        public double YMax { get; }
        public string Srs { get; }

        public string IXMin => InvariantDouble(XMin);
        public string IXMax => InvariantDouble(XMax);
        public string IYMin => InvariantDouble(YMin);
        public string IYMax => InvariantDouble(YMax);

        public BoundingBox(double xMin, double yMin, double xMax, double yMax, string srs)
        {
            //if (xMin >= xMax || yMin >= yMax)
            //    throw new InvalidOperationException("Максимальные координаты должны быть больше минимальных");
            XMin = Math.Min(xMin, xMax);
            YMin = Math.Min(yMin, yMax);
            XMax = Math.Max(xMin, xMax);
            YMax = Math.Max(yMin, yMax);
            Srs = srs;
        }

        public string ToWkt()
        {
            return $"POLYGON(({IXMin} {IYMin},{IXMax} {IYMin},{IXMax} {IYMax},{IXMin} {IYMax},{IXMin} {IYMin}))";
        }

        public override string ToString()
        {
            var bBox = $"{IXMin},{IYMin},{IXMax},{IYMax},{Srs}";
            return bBox;
        }

        /// <summary>
        /// Строковое представление без системы координат.
        /// </summary>
        /// <returns> Строковое представление. </returns>
        public string ToStringWithoutSrc()
        {
            var bBox = $"{IXMin},{IYMin},{IXMax},{IYMax}";
            return bBox;
        }

        /// <summary>
        /// Строковое представление координат с определенным разрядом.
        /// </summary>
        /// <param name="count"> Разряд. </param>
        /// <returns> Строка. </returns>
        public string ToBigString(int count = 10)
        {
            return XMin.ToString($"F{count}", CultureInfo.InvariantCulture) + "," +
                YMin.ToString($"F{count}", CultureInfo.InvariantCulture) + "," +
                XMax.ToString($"F{count}", CultureInfo.InvariantCulture) + "," +
                YMax.ToString($"F{count}", CultureInfo.InvariantCulture);
        }

        private static string InvariantDouble(double d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }
    }
}
