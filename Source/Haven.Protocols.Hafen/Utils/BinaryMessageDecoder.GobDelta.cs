using System;
using System.Collections.Generic;
using Haven.Protocols.Hafen.Messages;
using Haven.Utils;

namespace Haven.Protocols.Hafen.Utils
{
	internal static partial class BinaryMessageDecoder
	{
		public static GobDelta.Position ReadGobPosition(this BinaryDataReader reader)
		{
			var c = reader.ReadInt32Coord();
			var a = reader.ReadUInt16();
			return new GobDelta.Position
			{
				Coord = c,
				Angle = (a / 65536.0) * Math.PI * 2
			};
		}

		public static GobDelta.Resource ReadGobResource(this BinaryDataReader reader)
		{
			int resId = reader.ReadUInt16();

			byte[] data;
			if ((resId & 0x8000) != 0)
			{
				resId &= ~0x8000;
				var len = reader.ReadByte();
				data = reader.ReadBytes(len);
			}
			else
			{
				data = new byte[0];
			}

			return new GobDelta.Resource { Id = resId, Data = data };
		}

		public static GobDelta.StartMovement ReadGobStartMovement(this BinaryDataReader reader)
		{
			return new GobDelta.StartMovement
			{
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
			return new GobDelta.Speech
			{
				Offset = reader.ReadInt16() / 100.0f,
				Text = reader.ReadCString()
			};
		}

		public static GobDelta.ZOffset ReadGobZOffset(this BinaryDataReader reader)
		{
			return new GobDelta.ZOffset { Value = reader.ReadInt16() / 100.0f };
		}

		public static GobDelta.Light ReadGobLight(this BinaryDataReader reader)
		{
			return new GobDelta.Light
			{
				Offset = reader.ReadInt32Coord(),
				Size = reader.ReadUInt16(),
				Intensity = reader.ReadByte()
			};
		}

		public static GobDelta.Follow ReadGobFollow(this BinaryDataReader reader)
		{
			long oid = reader.ReadUInt32();
			if (oid != 0xffffffffL)
			{
				var resId = reader.ReadUInt16();
				var name = reader.ReadCString();
				return new GobDelta.Follow
				{
					GobId = oid,
					ResId = resId,
					Name = name
				};
			}
			return new GobDelta.Follow { GobId = oid };
		}

		public static GobDelta.Homing ReadGobHoming(this BinaryDataReader reader)
		{
			long oid = reader.ReadUInt32();
			switch (oid)
			{
				case 0xffffffffL:
					return new GobDelta.Homing { GobId = oid };
				default:
					return new GobDelta.Homing
					{
						GobId = oid,
						Target = reader.ReadInt32Coord(),
						Velocity = reader.ReadUInt16()
					};
			}
		}

		public static GobDelta.Overlay ReadGobOverlay(this BinaryDataReader reader)
		{
			int overlayId = reader.ReadInt32();
			var prs = (overlayId & 1) != 0;
			overlayId >>= 1;

			int resId = reader.ReadUInt16();

			byte[] data = null;
			if (resId != 65535)
			{
				if ((resId & 0x8000) != 0)
				{
					resId &= ~0x8000;
					var len = reader.ReadByte();
					data = reader.ReadBytes(len);
				}
				else
					data = new byte[0];
			}
			else
				resId = -1;

			return new GobDelta.Overlay
			{
				Id = overlayId,
				IsPersistent = prs,
				ResourceId = resId,
				SpriteData = data
			};
		}

		public static GobDelta.Health ReadGobHealth(this BinaryDataReader reader)
		{
			return new GobDelta.Health { Value = reader.ReadByte() };
		}

		public static GobDelta.Buddy ReadGobBuddy(this BinaryDataReader reader)
		{
			var name = reader.ReadCString();
			byte group = 0;
			byte type = 0;
			if (!string.IsNullOrEmpty(name))
			{
				group = reader.ReadByte();
				type = reader.ReadByte();
			}
			return new GobDelta.Buddy { Name = name, Group = group, Type = type };
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

		public static GobDelta.Composite ReadGobComposite(this BinaryDataReader reader)
		{
			return new GobDelta.Composite { ResourceId = reader.ReadUInt16() };
		}

		public static GobDelta.CompositePose ReadGobCompositePose(this BinaryDataReader reader)
		{
			var poses = new GobDelta.CompositePose();
			int flags = reader.ReadByte();
			poses.Seq = reader.ReadByte();
			poses.Interpolate = (flags & 1) != 0;
			if ((flags & 2) != 0)
			{
				poses.Poses = ReadResData(reader);
			}
			if ((flags & 4) != 0)
			{
				poses.TPoses = ReadResData(reader);
				poses.Time = (reader.ReadByte() / 10.0f);
			}
			return poses;
		}

		public static GobDelta.CompositeModel ReadGobCompositeModel(this BinaryDataReader reader)
		{
			while (true)
			{
				int modelId = reader.ReadUInt16();
				if (modelId == 65535)
					break;
				while (true)
				{
					int resId = reader.ReadUInt16();
					if (resId == 65535)
						break;
					if ((resId & 0x8000) != 0)
					{
						resId &= ~0x8000;
						reader.ReadBytes(reader.ReadByte()); // sdt
					}
				}
			}
			return new GobDelta.CompositeModel();
		}

		public static GobDelta.CompositeEqu ReadGobCompositeEqu(this BinaryDataReader reader)
		{
			while (true)
			{
				int h = reader.ReadByte();
				if (h == 255)
					break;
				int ef = h & 0x80;
				int et = h & 0x7f;
				string at = reader.ReadCString();
				int resId = reader.ReadUInt16();
				if ((resId & 0x8000) != 0)
				{
					resId &= ~0x8000;
					reader.ReadBytes(reader.ReadByte());
				}
				Point3F off;
				if ((ef & 128) != 0)
				{
					int x = reader.ReadInt16();
					int y = reader.ReadInt16();
					int z = reader.ReadInt16();
					off = new Point3F(x / 1000.0f, y / 1000.0f, z / 1000.0f);
				}
				else
				{
					off = Point3F.Empty;
				}
			}
			return new GobDelta.CompositeEqu();
		}

		public static GobDelta.Icon ReadGobIcon(this BinaryDataReader reader)
		{
			int resId = reader.ReadUInt16();
			if (resId != 65535)
				reader.ReadByte(); // not used flag
			return new GobDelta.Icon { ResourceId = resId };
		}

		public static GobDelta ReadResAttr(this BinaryDataReader reader)
		{
			reader.ReadUInt16(); // resid
			int len = reader.ReadByte();
			if (len > 0)
				reader.ReadBytes(len);
			return null;
		}

		public static GobDelta.Resource[] ReadResData(this BinaryDataReader reader)
		{
			var list = new List<GobDelta.Resource>();
			while (true)
			{
				int resId = reader.ReadUInt16();
				if (resId == 65535)
					break;

				byte[] data;
				if ((resId & 0x8000) != 0)
				{
					resId &= ~0x8000;
					var len = reader.ReadByte();
					data = reader.ReadBytes(len);
				}
				else
				{
					data = new byte[0];
				}
				list.Add(new GobDelta.Resource { Id = resId, Data = data });
			}
			return list.ToArray();
		}
	}
}
