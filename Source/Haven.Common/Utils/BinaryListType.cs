using System;
using System.Collections.Generic;

namespace Haven.Utils
{
	public enum BinaryListType
	{
		End = 0,
		Int32 = 1,
		String = 2,
		Coord = 3,
		Byte = 4,
		UInt16 = 5,
		Color = 6,
		List = 8,
		SByte = 9,
		Int16 = 10,
		Nil = 12,
		Uid = 13,
		Bytes = 14,
		Single = 15,
		Double = 16
	}

	public static class BinaryListTypes
	{
		private static readonly Dictionary<Type, BinaryListType> Mapping =
			new Dictionary<Type, BinaryListType> {
				{typeof(byte), BinaryListType.Byte },
				{typeof(byte[]), BinaryListType.Bytes },
				{typeof(Color), BinaryListType.Color },
				{typeof(double), BinaryListType.Double },
				{typeof(short), BinaryListType.Int16 },
				{typeof(int), BinaryListType.Int32 },
				{typeof(object[]), BinaryListType.List },
				{typeof(Point2D), BinaryListType.Coord },
				{typeof(sbyte), BinaryListType.SByte },
				{typeof(float), BinaryListType.Single },
				{typeof(string), BinaryListType.String },
				{typeof(ushort), BinaryListType.UInt16 },
		};

		public static BinaryListType TypeOf(object obj)
		{
			if (obj == null)
				return BinaryListType.Nil;
			BinaryListType type;
			if (Mapping.TryGetValue(obj.GetType(), out type))
				return type;
			throw new Exception($"Unsupported list item type {obj.GetType().FullName}");
		}
	}
}
