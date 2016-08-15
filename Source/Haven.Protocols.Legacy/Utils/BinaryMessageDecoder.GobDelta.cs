using System.Collections.Generic;
using Haven.Protocols.Legacy.Messages;
using Haven.Utils;

namespace Haven.Protocols.Legacy.Utils
{
	internal static partial class BinaryMessageDecoder
	{
		public static GobDelta.Position ReadGobPosition(this BinaryDataReader reader)
		{
			return new GobDelta.Position { Value = reader.ReadInt32Coord() };
		}

		public static GobDelta.Resource ReadGobResource(this BinaryDataReader reader)
		{
			int resId = reader.ReadUInt16();

			byte[] spriteData;
			if ((resId & 0x8000) != 0)
			{
				resId &= ~0x8000;
				var len = reader.ReadByte();
				spriteData = reader.ReadBytes(len);
			}
			else
			{
				spriteData = new byte[0];
			}

			return new GobDelta.Resource { Id = resId, SpriteData = spriteData };
		}

		public static GobDelta.StartMovement ReadGobStartMovement(this BinaryDataReader reader)
		{
			return new GobDelta.StartMovement {
				Origin = reader.ReadInt32Coord(),
				Destination = reader.ReadInt32Coord(),
				TotalSteps = reader.ReadInt32()
			};
		}

		public static GobDelta.AdjustMovement ReadGobAdjustMovement(this BinaryDataReader reader)
		{
			return new GobDelta.AdjustMovement { Step = reader.ReadInt32() };
		}

		public static GobDelta.Speech ReadGobSpeech(this BinaryDataReader reader)
		{
			return new GobDelta.Speech {
				Offset = reader.ReadInt32Coord(),
				Text = reader.ReadCString()
			};
		}

		public static GobDelta.DrawOffset ReadGobDrawOffset(this BinaryDataReader reader)
		{
			return new GobDelta.DrawOffset { Value = reader.ReadInt32Coord() };
		}

		public static GobDelta.Light ReadGobLight(this BinaryDataReader reader)
		{
			return new GobDelta.Light {
				Offset = reader.ReadInt32Coord(),
				Size = reader.ReadUInt16(),
				Intensity = reader.ReadByte()
			};
		}

		public static GobDelta.Follow ReadGobFollow(this BinaryDataReader reader)
		{
			int oid = reader.ReadInt32();
			if (oid != -1)
			{
				return new GobDelta.Follow {
					GobId = oid,
					Szo = reader.ReadByte(),
					Offset = reader.ReadInt32Coord()
				};
			}
			return new GobDelta.Follow { GobId = oid };
		}

		public static GobDelta.Homing ReadGobHoming(this BinaryDataReader reader)
		{
			int oid = reader.ReadInt32();
			if (oid != -1)
			{
				return new GobDelta.Homing {
					GobId = oid,
					Target = reader.ReadInt32Coord(),
					Velocity = reader.ReadUInt16()
				};
			}
			return new GobDelta.Homing { GobId = oid };
		}

		public static GobDelta.Overlay ReadGobOverlay(this BinaryDataReader reader)
		{
			int overlayId = reader.ReadInt32();
			var prs = (overlayId & 1) != 0;
			overlayId >>= 1;

			int resId = reader.ReadUInt16();
			
			byte[] spriteData = null;
			if (resId != 65535)
			{
				if ((resId & 0x8000) != 0)
				{
					resId &= ~0x8000;
					var len = reader.ReadByte();
					spriteData = reader.ReadBytes(len);
				}
				else
					spriteData = new byte[0];
			}
			else
				resId = -1;

			return new GobDelta.Overlay {
				Id = overlayId,
				IsPersistent = prs,
				ResourceId = resId,
				SpriteData = spriteData
			};
		}

		public static GobDelta.Health ReadGobHealth(this BinaryDataReader reader)
		{
			return new GobDelta.Health { Value = reader.ReadByte() };
		}

		public static GobDelta.Buddy ReadGobBuddy(this BinaryDataReader reader)
		{
			return new GobDelta.Buddy {
				Name = reader.ReadCString(),
				Group = reader.ReadByte(),
				Type = reader.ReadByte()
			};
		}

		public static GobDelta.Avatar ReadGobAvatar(this BinaryDataReader reader)
		{
			var layers = new List<int>();
			while (true)
			{
				int layer = reader.ReadUInt16();
				if (layer == 65535)
					break;
				layers.Add(layer);
			}
			return new GobDelta.Avatar { ResourceIds = layers.ToArray() };
		}

		public static GobDelta.Layers ReadGobLayers(this BinaryDataReader reader)
		{
			int baseResId = reader.ReadUInt16();
			var layers = new List<int>();
			while (true)
			{
				int layer = reader.ReadUInt16();
				if (layer == 65535)
					break;
				layers.Add(layer);
			}
			return new GobDelta.Layers {
				BaseResourceId = baseResId,
				ResourceIds = layers.ToArray()
			};
		}
	}
}
