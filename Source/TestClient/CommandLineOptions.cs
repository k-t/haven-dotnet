using System.Collections.Generic;

namespace TestClient
{
	public class CommandLineOptions
	{
		public string Host
		{
			get;
			private set;
		}

		public string User
		{
			get;
			private set;
		}

		public string Password
		{
			get;
			private set;
		}

		public bool UseLegacyProtocol
		{
			get;
			private set;
		}

		public static CommandLineOptions Parse(string[] args)
		{
			if (args.Length == 0)
				return null;

			var options = new CommandLineOptions();
			foreach (var arg in args)
			{
				if (arg.StartsWith("-"))
				{
					var kv = Parse(arg);
					switch (kv.Key)
					{
						case "user":
							options.User = kv.Value;
							break;
						case "pass":
							options.Password = kv.Value;
							break;
						case "l":
							options.UseLegacyProtocol = true;
							break;
						default:
							return null;
					}
				}
				else
				{
					options.Host = arg;
				}
			}
			return options;
		}

		private static KeyValuePair<string, string> Parse(string arg)
		{
			var parts = arg.Split('=');

			var key = parts.Length > 0
				? parts[0].TrimStart('-')
				: null;

			var value = parts.Length > 1
				? parts[1]
				: null;

			return new KeyValuePair<string, string>(key, value);
		}
	}
}