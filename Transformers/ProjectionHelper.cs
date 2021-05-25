using Proj4Net;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Common;

namespace WebApp.Transformers
{
    public class ProjectionHelper
    {
		/// <summary>
		/// Текущая система координат.
		/// </summary>
		private readonly CoordinateReferenceSystem _currentProj;

		/// <summary>
		/// Результирующая система координат.
		/// </summary>
		private readonly CoordinateReferenceSystem _projectTo;

		/// <summary>
		/// Список промежуточных СК.
		/// </summary>
		private readonly IEnumerable<CoordinateReferenceSystem> _intermediateProjections;

		private readonly IGeometryFactory _geometryFactory;

		private readonly CoordinateTransformFactory _coordinateTransformFactory;

		/// <summary>
		/// Получение объекта проекции.
		/// </summary>
		/// <param name="fromEpsgCode"> Код EPSG исходной СК. (может быть null для анонимной СК). </param>
		/// <param name="fromParamStr"> PROJ.4 проекция (Например: "+proj=aea +lat_1=50 +lat_2=58.5 +lat_0=45 +lon_0=-126 +x_0=1000000 +y_0=0 +ellps=GRS80 +units=m") - datum. </param>
		/// <param name="toEpsgCode"> Код EPSG результирующей СК (может быть null для анонимной СК).. </param>
		/// <param name="toParamStr"> PROJ.4 проекция (Например: "+proj=aea +lat_1=50 +lat_2=58.5 +lat_0=45 +lon_0=-126 +x_0=1000000 +y_0=0 +ellps=GRS80 +units=m") - datum. </param>
		/// <param name="intermediateProjections"> Инструкция для промежуточного перепроецирования. По умолчанию - EPSG:4326. </param>
		private ProjectionHelper(string fromEpsgCode, string toEpsgCode, string fromParamStr = null, string toParamStr = null, IEnumerable<CoordinateReferenceSystem> intermediateProjections = null)
		{
			var crFactory = new CoordinateReferenceSystemFactory();

			_currentProj = fromParamStr != null
				? crFactory.CreateFromParameters(fromEpsgCode, fromParamStr)
				: crFactory.CreateFromName(fromEpsgCode);

			_projectTo = toParamStr != null
				? crFactory.CreateFromParameters(toEpsgCode, toParamStr)
				: crFactory.CreateFromName(toEpsgCode);

			_intermediateProjections = intermediateProjections ??
									   new List<CoordinateReferenceSystem> { crFactory.CreateFromName("EPSG:4326") };

			_geometryFactory = (IGeometryFactory) new GeometryFactory();//TODO: не явное преобразование
			_coordinateTransformFactory = new CoordinateTransformFactory();
		}

		/// <summary>
		/// Структура для хранения ключей кэша конвертеров проекций
		/// </summary>
		private struct ProjectionCacheKey
		{
			private readonly string _fromEpsgCode;

			private readonly string _toEpsgCode;

			private readonly IEnumerable<CoordinateReferenceSystem> _intermediateProjections;

			public ProjectionCacheKey(string fromEpsgCode, string toEpsgCode, IEnumerable<CoordinateReferenceSystem> intermediateProjections)
			{
				_fromEpsgCode = fromEpsgCode;
				_toEpsgCode = toEpsgCode;
				_intermediateProjections = intermediateProjections;
			}

			public bool Equals(ProjectionCacheKey other)
			{
				var into = _intermediateProjections != null
					? string.Join("", _intermediateProjections.Select(x => x.GetParameterString()))
					: string.Empty;

				var outto = _intermediateProjections != null
					? string.Join("", other._intermediateProjections.Select(x => x.GetParameterString()))
					: string.Empty;

				return !string.Equals(_fromEpsgCode, other._fromEpsgCode) || !string.Equals(_toEpsgCode, other._toEpsgCode) || into == outto;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				return obj is ProjectionCacheKey && Equals((ProjectionCacheKey)obj);
			}

			public override int GetHashCode()
			{
				var hash = _intermediateProjections != null
					? string.Join("", _intermediateProjections.Select(x => x.GetParameterString())).GetHashCode()
					: 0;

				unchecked
				{
					return ((_fromEpsgCode != null ? _fromEpsgCode.GetHashCode() : 0) * 397) ^
						(_toEpsgCode != null ? _toEpsgCode.GetHashCode() : 0) ^ hash;
				}
			}
		}

		/// <summary>
		/// Кэш конвертеров проекций
		/// </summary>
		private static readonly Dictionary<ProjectionCacheKey, ProjectionHelper> _projectorCache = new Dictionary<ProjectionCacheKey, ProjectionHelper>();

		/// <summary>
		/// Предоставляет экземпляр ProjectionHelper для пары проекций
		/// </summary>
		/// <param name="fromEpsgCode">Исходная проекция</param>
		/// <param name="toEspgCode">Целевая проекция</param>
		/// <param name="intermediateProjections"> Инструкция для промежуточного перепроецирования. По умолчанию - EPSG:4326. </param>
		/// <returns>Конвертер проекций</returns>
		public static ProjectionHelper GetProjectionHelper(string fromEpsgCode, string toEspgCode, 
			IEnumerable<CoordinateReferenceSystem> intermediateProjections = null)
		{
			lock (_projectorCache)
			{
				var key = new ProjectionCacheKey(fromEpsgCode, toEspgCode, intermediateProjections);

				ProjectionHelper projectionHelper;

				if (_projectorCache.ContainsKey(key))
				{
					projectionHelper = _projectorCache[key];
				}
				else
				{
					projectionHelper = new ProjectionHelper(fromEpsgCode, toEspgCode, intermediateProjections: intermediateProjections);
					_projectorCache.Add(key, projectionHelper);
				}

				return projectionHelper;
			}
		}

		/// <inheritdoc />
		public IGeometry ReprojectGeometry(IGeometry source)
		{
			if (source == null)
				return null;

			switch (source.OgcGeometryType)
			{
				case GeoAPI.Geometries.OgcGeometryType.Point:
					return _geometryFactory.CreatePoint(MakeProject(source.Coordinate));

				case GeoAPI.Geometries.OgcGeometryType.LineString:
					return _geometryFactory.CreateLineString(MakeProject(source.Coordinates));

				case GeoAPI.Geometries.OgcGeometryType.Polygon:
					return ReprojectPolygon((IPolygon)source);

				case GeoAPI.Geometries.OgcGeometryType.MultiPoint:
				case GeoAPI.Geometries.OgcGeometryType.MultiLineString:
				case GeoAPI.Geometries.OgcGeometryType.MultiPolygon:
				case GeoAPI.Geometries.OgcGeometryType.GeometryCollection:
					var reprojectedGeometries = new List<IGeometry>();
					for (var i = 0; i < source.NumGeometries; i++)
						reprojectedGeometries.Add(ReprojectGeometry(source.GetGeometryN(i)));

					return _geometryFactory.BuildGeometry(reprojectedGeometries);

				default:
					throw new InvalidOperationException("Неподдерживаемый тип геометрии");
			}
		}

		/// <summary>
		/// Выполнить перепроецирование массива координат.
		/// </summary>
		/// <param name="coordinates">Массив координат.</param>
		/// <returns>Перепроецированный массив.</returns>
		public GeoAPI.Geometries.Coordinate[] MakeProject(GeoAPI.Geometries.Coordinate[] coordinates)
		{
			try
			{
				var result = new GeoAPI.Geometries.Coordinate[coordinates.Length];
				for (var i = 0; i < coordinates.Length; i++)
					result[i] = MakeProject(coordinates[i]);

				return result;
			}
			catch (Exception e)
			{
				Logger.Error("Ошибка перепроецирования геометрии", e);
				throw;
			}
		}

		/// <summary>
		/// Выполнить промежуточное перепроецирование.
		/// </summary>
		/// <param name="factory"></param>
		/// <param name="coordinate"></param>
		/// <returns></returns>
		private GeoAPI.Geometries.Coordinate MakeProject(GeoAPI.Geometries.Coordinate coordinate)
        {
            var result = coordinate;
            var projections = new List<CoordinateReferenceSystem>
            {
                _currentProj
            };
            projections.AddRange(_intermediateProjections);
            projections.Add(_projectTo);

            for (var i = 0; i < projections.Count - 1; i++)
            {
                var from = projections[i];
                var to = projections[i + 1];
                result = MakeProject(result, from, to);
            }

            return result;
        }

		/// <summary>
		/// Перепроецирование полигона.
		/// </summary>
		/// <param name="source">Исходный полигон. </param>
		/// <returns>Перепроецированный полигон. </returns>
		private IPolygon ReprojectPolygon(IPolygon source)
		{
			var ring = ReprojectLinearRing(source.Shell);

			var holes = new ILinearRing[source.Holes.Length];
			for (var i = 0; i < source.Holes.Length; i++)
				holes[i] = ReprojectLinearRing(source.Holes[i]);

			return _geometryFactory.CreatePolygon(ring, holes);
		}

		/// <summary>
		/// Осуществить трансформацию.
		/// </summary>
		/// <param name="factory"> Фабрика. </param>
		/// <param name="coordinate"> Координаты. </param>
		/// <param name="source"> СК-источник. </param>
		/// <param name="target"> СК-цель. </param>
		/// <returns> Новые координаты. </returns>
		private GeoAPI.Geometries.Coordinate MakeProject(GeoAPI.Geometries.Coordinate coordinate, CoordinateReferenceSystem source,
			CoordinateReferenceSystem target)
		{
			var result = new GeoAPI.Geometries.Coordinate();
			var transform = _coordinateTransformFactory.CreateTransform(source, target);
			transform.Transform(coordinate, result);

			return result;
		}

		/// <summary>
		/// Перепроецирование замкнуцтой линии. 
		/// </summary>
		/// <param name="source">Исходная замкнутая линия. </param>
		/// <returns>Перепроецированная замкнутая линия. </returns>
		private ILinearRing ReprojectLinearRing(ILinearRing source)
		{
			return _geometryFactory.CreateLinearRing(MakeProject(source.Coordinates));
		}
	}
}
