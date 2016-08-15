using System;
using System.Globalization;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini
{
	public static class IniKeyCollectionExtensions
	{
		public static void Add(this IniKeyCollection keys, string key, char value)
		{
			keys.Add(key, value.ToString());
		}

		public static void Add(this IniKeyCollection keys, string key, int value)
		{
			keys.Add(key, $"{value:D}");
		}

		public static void Add(this IniKeyCollection keys, string key, Point2D value)
		{
			keys.Add(key, $"{value.X:D},{value.Y:D}");
		}

		public static void Add(this IniKeyCollection keys, string key, double value)
		{
			keys.Add(key, value.ToString(CultureInfo.InvariantCulture));
		}

		public static bool GetBool(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			return bool.Parse(value);
		}

		public static bool GetBool(this IniKeyCollection keys, string key, bool defaultValue)
		{
			bool value;
			var strValue = keys[key]?.Value ?? "";
			return bool.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static byte GetByte(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			return byte.Parse(value);
		}

		public static byte GetByte(this IniKeyCollection keys, string key, byte defaultValue)
		{
			byte value;
			var strValue = keys[key]?.Value ?? "";
			return byte.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static char GetChar(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			if (value.Length == 1)
				return value[0];
			throw new FormatException("Expected single character");
		}

		public static double GetDouble(this IniKeyCollection keys, string key, double defaultValue)
		{
			var value = keys[key]?.Value;
			return (value != null)
				? double.Parse(value, CultureInfo.InvariantCulture)
				: defaultValue;
		}

		public static T GetEnum<T>(this IniKeyCollection keys, string key)
			where T : struct
		{
			var value = keys.GetString(key);
			return (T)Enum.Parse(typeof(T), value, true);
		}

		public static T GetEnum<T>(this IniKeyCollection keys, string key, T defaultValue)
			where T : struct
		{
			var value = keys.GetString(key, "");
			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		public static char GetChar(this IniKeyCollection keys, string key, char defaultValue)
		{
			var value = keys[key]?.Value ?? "";
			return value.Length > 0 ? value[0] : defaultValue;
		}

		public static short GetInt16(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			return short.Parse(value);
		}

		public static short GetInt16(this IniKeyCollection keys, string key, short defaultValue)
		{
			short value;
			var strValue = keys[key]?.Value ?? "";
			return short.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static int GetInt32(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			return int.Parse(value);
		}

		public static int GetInt32(this IniKeyCollection keys, string key, int defaultValue)
		{
			int value;
			var strValue = keys[key]?.Value ?? "";
			return int.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static ushort GetUInt16(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value ?? "";
			return ushort.Parse(value);
		}

		public static ushort GetUInt16(this IniKeyCollection keys, string key, ushort defaultValue)
		{
			ushort value;
			var strValue = keys[key]?.Value ?? "";
			return ushort.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static Point2D GetPoint(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value;
			if (value == null)
				throw new FormatException($"Missing key: {key}");
			var parts = value.Split(',');
			if (parts.Length != 2)
				throw new FormatException($"Invalid point format: {value}");
			return new Point2D(int.Parse(parts[0]), int.Parse(parts[1]));
		}

		public static Point2D GetPoint(this IniKeyCollection keys, string key, Point2D defaultValue)
		{
			return keys.Contains(key) ? keys.GetPoint(key) : defaultValue;
		}

		public static string GetString(this IniKeyCollection keys, string key)
		{
			var value = keys[key]?.Value;
			if (value == null)
				throw new FormatException($"Missing key: {key}");
			return value;
		}

		public static string GetString(this IniKeyCollection keys, string key, string defaultValue)
		{
			return keys[key]?.Value ?? defaultValue;
		}
	}
}
