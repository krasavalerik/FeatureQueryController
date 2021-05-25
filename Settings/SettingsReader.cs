using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Settings
{
    public class SettingsReader
    {
		/// <summary>
		/// Экземпляр класса конфига.
		/// </summary>
		private static SettingsReader _instance;

		/// <summary>
		/// Параметры гис-систем.
		/// </summary>
		private readonly GisConfigValues _gisConfigValues;

		/// <summary>
		/// Возвращает экземпляр класса конфига.
		/// </summary>
		public static SettingsReader Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SettingsReader();

				}

				return _instance;
			}
		}

		/// <summary>
		/// Параметры гис-систем.
		/// </summary>
		internal GisConfigValues GisConfigValues => Instance._gisConfigValues;

		///// <summary>
		///// Инициализирует новый экземпляр класса <see cref="SettingsReader"/>.
		///// </summary>
		///// <param name="filePath"> Путь к файлу концигурации. </param>
		//private SettingsReader(string filePath = null)
		//{
		//	if (string.IsNullOrWhiteSpace(filePath))
		//	{
		//		filePath = ServerEnvironment.GetAssemblyFolder();
		//		filePath = Path.Combine(filePath, "BLL\\Settings\\Cgis3Settings.xml");
		//	}

		//	if (!File.Exists(filePath))
		//	{
		//		throw new ArgumentException($"Не обнаружен файл настроек по пути: {filePath}");
		//	}

		//	var doc = new XmlDocument();
		//	doc.Load(filePath);

		//	// чтение настроек гис-систем.
		//	try
		//	{
		//		_geoPortalSettings = SimpleXmlReader.DeserializeFromBin<GeoPortalSettings>(GEO_PORTAL_SETTINGS);


		//		_gisConfigValues = GisConfigValues.Load(doc);

		//		// чтение настроек внешних модулей.
		//		_internalModulesConfigValues = new InternalModulesConfigValues(doc);

		//		// чтение настроек модулей внешних ресурсов.
		//		_externalModulesConfigValues = new ExternalResourceModulesConfigValues(doc);

		//		_floodFillConfigValues = new FloodFillConfigValues(doc);

		//		EscapeCharConfigValues = new EscapeCharConfigValues(doc);

		//		RouteColorValues = new RouteColorValues(doc);
		//	}
		//	catch (Exception exception)
		//	{
		//		Logger.Fatal("Ошибка при чтении файла конфигурации ГИСП3.", exception);
		//		throw;
		//	}

		//	// чтение глобальных настроек
		//	//TODO Возможно, лучше сделать загрузку аналогично загрузке конфигов ГИСП3
		//	GlobalConfigValues = new GlobalConfigValues(doc.SelectSingleNode("settings/globalSettings"));
		//}
	}
}
