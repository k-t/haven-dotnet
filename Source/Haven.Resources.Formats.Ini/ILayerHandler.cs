using System;
using System.Collections.Generic;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini
{
	public interface ILayerHandler
	{
		Type DataType { get; }

		string SectionName { get; }

		IEnumerable<string> ExternalFileKeys { get; }

		string GetExternalFileExtension(string externalFileKey, object data);

		object Load(IniKeyCollection iniData, LayerHandlerContext context);

		void Save(IniKeyCollection iniData, object data, LayerHandlerContext context);
	}
}
