using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WebApp.Settings
{
    /// <summary>
	/// Класс со значениями настроек гис-систем.
	/// </summary>
    public class GisConfigValues
    {
		/// <summary>
		/// Наименование атриубта с глобальным идентификатором.
		/// </summary>
		internal readonly string GlobalIdFieldName;

		/// <summary>
		/// Список служебных полей.
		/// </summary>
		internal readonly HashSet<string> SystemFieldNames = new HashSet<string>();

		/// <summary>
		/// Список алиасов атриубта с глобальным идентификатором.
		/// </summary>
		internal readonly HashSet<string> GlobalIdFieldAliases = new HashSet<string>();

		/// <summary>
		/// Список атриубтов площади.
		/// </summary>
		internal readonly HashSet<string> AreaAttributesName = new HashSet<string>();

		/// <summary>
		/// Конструктор настроек гис-систем.
		/// </summary>
		/// <param name="globalIdFieldName"></param>
		/// <param name="systemFieldNames"></param>
		/// <param name="globalIdFieldAliases"></param>
		/// <param name="areaAttributesName"></param>
		internal GisConfigValues(string globalIdFieldName, IEnumerable<string> systemFieldNames, IEnumerable<string> globalIdFieldAliases, IEnumerable<string> areaAttributesName)
		{
			GlobalIdFieldName = globalIdFieldName;

			SystemFieldNames.UnionWith(systemFieldNames);
			SystemFieldNames.UnionWith(globalIdFieldAliases);
			SystemFieldNames.Add(globalIdFieldName);

			GlobalIdFieldAliases.UnionWith(globalIdFieldAliases);

			AreaAttributesName.UnionWith(areaAttributesName);
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="GisConfigValues"/>.
		/// </summary>
		/// <param name="document"></param>
		internal static GisConfigValues Load(XmlDocument document)
		{
			var node = document.SelectSingleNode("settings/gisSettings");

			if (node == null)
			{
				throw new Exception("Не найден узел с параметрами гис-систем в файле настроек (gisSettings).");
			}

			var globalIdFieldName = node.SelectSingleNode("globalIdFieldName").Attributes["value"].InnerText;

			var systemFieldNamesTag = node.SelectSingleNode("layerSystemFieldsNames");
			var systemFieldNames = from XmlNode nod in systemFieldNamesTag
								   where nod.Attributes != null
								   select nod.Attributes["value"].InnerText;

			var globalIdFieldAliasesTag = node.SelectSingleNode("globalIdFieldAliases");
			var globalIdFieldAliases = from XmlNode nod in globalIdFieldAliasesTag
									   where nod.Attributes != null
									   select nod.Attributes["value"].InnerText;

			var areaAttributesNameTag = node.SelectSingleNode("areaAttributesName");
			var areaAttributesName = from XmlNode nod in areaAttributesNameTag
									 where nod.Attributes != null
									 select nod.Attributes["value"].InnerText;

			var config = new GisConfigValues(globalIdFieldName, systemFieldNames, globalIdFieldAliases, areaAttributesName);

			return config;
		}
	}
}
