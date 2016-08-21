using System;
using System.Collections.Generic;
using System.Linq;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini
{
	public abstract class GenericLayerHandler<T> : ILayerHandler
		where T : class
	{
		private readonly string sectionName;

		protected GenericLayerHandler(string sectionName)
		{
			this.sectionName = sectionName;
		}

		public Type DataType
		{
			get { return typeof(T); }
		}

		public string SectionName
		{
			get { return sectionName; }
		}

		public virtual IEnumerable<string> ExternalFileKeys
		{
			get { return Enumerable.Empty<string>(); }
		}

		protected virtual string GetExternalFileExtension(string externalFileKey, T data)
		{
			throw new ArgumentException($"Unknown external file key '{externalFileKey}'", nameof(externalFileKey));
		}

		protected abstract T Load(IniKeyCollection iniData, LayerHandlerContext context);

		protected abstract void Save(IniKeyCollection iniData, T data, LayerHandlerContext context);

		#region IIniLayerHandler

		string ILayerHandler.GetExternalFileExtension(string externalFileKey, object data)
		{
			return GetExternalFileExtension(externalFileKey, (T)data);
		}

		object ILayerHandler.Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return Load(iniData, context);
		}

		void ILayerHandler.Save(IniKeyCollection iniData, object data, LayerHandlerContext context)
		{
			Save(iniData, (T)data, context);
		}

		#endregion
	}
}
