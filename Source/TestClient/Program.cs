using System;
using Haven.Net;
using Haven.Protocols.Hafen;
using Haven.Protocols.Legacy;

namespace TestClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var options = CommandLineOptions.Parse(args);
			if (options == null)
			{
				ShowUsage();
				return;
			}
			if (!ValidateOptions(options))
			{
				return;
			}

			GameClient client;
			if (options.UseLegacyProtocol)
			{
				client= new LegacyClient(GetLegacyConfig(options.Host));
				client.Messages.Subscribe(new LegacyMessageHandlerBase());
			}
			else
			{
				client = new HafenClient(GetHafenConfig(options.Host));
				client.Messages.Subscribe(new HafenMessageHandlerBase());
			}

			var authResult = client.Authenticate(options.User, options.Password, true);
			if (authResult.IsSuccessful)
			{
				Console.WriteLine("Authentication was successful!");
				Console.WriteLine("\tsessionId={0}", authResult.SessionId);
				Console.WriteLine("\tcookie={0}", Convert.ToBase64String(authResult.SessionCookie));
				Console.WriteLine("\ttoken={0}", Convert.ToBase64String(authResult.SessionToken));

				client.Messages.Subscribe(new TestLegacyMessageHandler(client));
				client.Connect();

				if (client.State != GameClientState.Connected)
				{
					Console.WriteLine("Error: client state is '{0}' while it should be connected!", client.State);
					return;
				}

				Console.WriteLine("Connected...");
				Console.ReadKey();
				client.Close();
			}
			else
			{
				Console.WriteLine("Authentication error");
			}
		}

		private static GameClientConfig GetLegacyConfig(string host)
		{
			host = string.IsNullOrEmpty(host) ? Config.DefaultLegacyHost : host;
			return new GameClientConfig {
				AuthServerAddress = new NetworkAddress(host, Config.DefaultLegacyAuthPort),
				GameServerAddress = new NetworkAddress(host, Config.DefaultLegacyGamePort)
			};
		}

		private static GameClientConfig GetHafenConfig(string host)
		{
			host = string.IsNullOrEmpty(host) ? Config.DefaultHafenHost : host;
			return new GameClientConfig {
				AuthServerAddress = new NetworkAddress(host, Config.DefaultHafenAuthPort),
				GameServerAddress = new NetworkAddress(host, Config.DefaultHafenGamePort)
			};
		}

		private static bool ValidateOptions(CommandLineOptions options)
		{
			if (string.IsNullOrEmpty(options.User))
			{
				Console.WriteLine("Error: User name is empty or missing");
				return false;
			}
			if (string.IsNullOrEmpty(options.Password))
			{
				Console.WriteLine("Error: User password is empty or missing");
				return false;
			}
			return true;
		}

		private static void ShowUsage()
		{
			const string usageLineFormat = "  {0,-20}{1,-10}";

			Console.WriteLine("Usage: TestClient -user=<username> -pass=<password> [-l] [<host>]");
			Console.WriteLine();
			Console.WriteLine("Options:");
			Console.WriteLine(usageLineFormat, "-user=<username>", "User name.");
			Console.WriteLine(usageLineFormat, "-pass=<password>", "User password.");
			Console.WriteLine(usageLineFormat, "-l", "Use legacy protocol.");
			Console.WriteLine(usageLineFormat, "  ", "If not specified, hafen protocol is used.");
			Console.WriteLine();
			Console.WriteLine("Example:");
			Console.WriteLine(usageLineFormat, "TestClient -user=foo -pass=bar -l legacy.havenandhearth.com", "");
			Console.WriteLine();
		}
	}
}
