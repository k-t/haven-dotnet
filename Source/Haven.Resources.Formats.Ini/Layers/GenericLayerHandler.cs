using System;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public abstract class GenericLayerHandler<T> : IIniLayerHandler
		where T : class
	{
		private readonly string sectionName;

		protected GenericLayerHandler(string sectionName)
		{
			this.sectionName = sectionName;
		}

		public string SectionName
		{
			get { return sectionName; }
		}

		public Type Type
		{
			get { return typeof(T); }
		}

		protected abstract void Init(IniLayer layer, T data);
		protected abstract void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource);
		protected abstract void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource);

		#region IIniLayerHandler

		IniLayer IIniLayerHandler.Create(object data)
		{
			var layer = new IniLayer();
			layer.Data = data;
			Init(layer, (T)data);
			return layer;
		}

		IniLayer IIniLayerHandler.Load(IniKeyCollection keys, IFileSource fileSource)
		{
			var layer = new IniLayer();
			Load(layer, keys, fileSource);
			return layer;
		}

		void IIniLayerHandler.Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			Save(layer, keys, fileSource);
		}

		#endregion
	}
}
