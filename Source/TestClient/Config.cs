using System.ComponentModel;
using System.Configuration;

namespace TestClient
{
	internal static class Config
	{
		public static string DefaultLegacyHost => ConfigurationManager.AppSettings[nameof(DefaultLegacyHost)];

		public static int DefaultLegacyAuthPort
		{
			get { return Get<int>(nameof(DefaultLegacyAuthPort)); }
		}

		public static int DefaultLegacyGamePort
		{
			get { return Get<int>(nameof(DefaultLegacyGamePort)); }
		}

		public static string DefaultHafenHost
		{
			get { return ConfigurationManager.AppSettings[nameof(DefaultHafenHost)]; }
		}

		public static int DefaultHafenAuthPort
		{
			get { return Get<int>(nameof(DefaultHafenAuthPort)); }
		}

		public static int DefaultHafenGamePort
		{
			get { return Get<int>(nameof(DefaultHafenGamePort)); }
		}

		private static T Get<T>(string key)
		{
			var appSetting = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrWhiteSpace(appSetting))
				return default(T);

			var converter = TypeDescriptor.GetConverter(typeof(T));
			return (T)converter.ConvertFromInvariantString(appSetting);
		}
	}
}
