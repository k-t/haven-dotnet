using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class CodeLayerHandler : GenericLayerHandler<CodeLayer>
	{
		private const string ClassFileKey = "class";

		public CodeLayerHandler() : base("code")
		{
		}

		protected override void Init(IniLayer layer, CodeLayer data)
		{
			layer.Files[ClassFileKey] = ".class";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var classFileName = keys.GetString("file");

			var code = new CodeLayer();
			code.Name = keys.GetString("name");
			code.ByteCode = fileSource.Read(classFileName);

			layer.Data = code;
			layer.Files[ClassFileKey] = classFileName;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var classFileName = layer.Files[ClassFileKey];

			var code = (CodeLayer)layer.Data;
			keys.Add("name", code.Name);
			keys.Add("file", classFileName);
			
			fileSource.Write(classFileName, code.ByteCode);
		}
	}
}
