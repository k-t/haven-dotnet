using System.Collections.Generic;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class CodeLayerHandler : GenericLayerHandler<CodeLayer>
	{
		private const string ClassFileKey = "class";
		private static readonly string[] FileKeys = { ClassFileKey };

		public CodeLayerHandler() : base("code")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, CodeLayer data)
		{
			switch (externalFileKey)
			{
				case ClassFileKey:
					return ".class";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override CodeLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new CodeLayer
			{
				Name = iniData.GetString("name"),
				ByteCode = context.LoadExternalFile(ClassFileKey)
			};
		}

		protected override void Save(IniKeyCollection iniData, CodeLayer data, LayerHandlerContext context)
		{
			iniData.Add("name", data.Name);

			context.SaveExternalFile(ClassFileKey, data.ByteCode);
		}
	}
}
