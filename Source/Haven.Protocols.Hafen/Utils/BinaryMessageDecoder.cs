using System;
using System.Collections.Generic;
using System.IO;
using Haven.Protocols.Hafen.Messages;
using Haven.Utils;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Haven.Protocols.Hafen.Utils
{
	internal static partial class BinaryMessageDecoder
	{
		public static UpdateCharAttributes ReadCharAttributesUpdateEvent(this BinaryDataReader reader)
		{
			var attributes = new List<CharAttribute>();
			while (reader.HasRemaining)
			{
				attributes.Add(new CharAttribute {
					Name = reader.ReadCString(),
					BaseValue = reader.ReadInt32(),
					ModifiedValue = reader.ReadInt32()
				});
			}
			return new UpdateCharAttributes { Attributes = attributes.ToArray() };
		}

		public static UpdateActions ReadGameActionsUpdateEvent(this BinaryDataReader reader)
		{
			var added = new List<ResourceRef>();
			var removed = new List<ResourceRef>();

			while (reader.HasRemaining)
			{
				var removeFlag = reader.ReadByte() == '-';
				var name = reader.ReadCString();
				var version = reader.ReadUInt16();

				if (!removeFlag)
				{
					int t;
					while ((t = reader.ReadByte()) != 0)
					{
						switch (t)
						{
							case '!':
								break;
							case '*':
								reader.ReadInt32(); // meter
								reader.ReadInt32(); // dtime
								break;
							case '^':
								break;
						}
					}
				}

				var list = removeFlag ? removed : added;
				list.Add(new ResourceRef(name, version));
			}

			return new UpdateActions {
				Added = added.ToArray(),
				Removed = removed.ToArray()
			};
		}

		public static MapInvalidateGrid ReadMapInvalidateGridEvent(this BinaryDataReader reader)
		{
			return new MapInvalidateGrid {
				Coord = reader.ReadInt32Coord()
			};
		}

		public static MapInvalidateRegion ReadMapInvalidateRegionEvent(this BinaryDataReader reader)
		{
			var ul = reader.ReadInt32Coord();
			var br = reader.ReadInt32Coord();
			return new MapInvalidateRegion {
				UpperLeft = ul,
				BottomRight = br
			};
		}

		public static MapUpdateGrid ReadMapUpdateEvent(this BinaryDataReader reader)
		{
			var ev = new MapUpdateGrid {
				Coord = reader.ReadInt32Coord(),
				MinimapName = reader.ReadCString(),
				Overlays = new int[100 * 100],
				Z = new short[100 * 100]
			};

			var pfl = new byte[256];
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = reader.ReadByte();
			}

			var blob = Unpack(reader.ReadRemaining());
			reader = new BinaryDataReader(blob);
			ev.Id = reader.ReadInt64();
			var tilesets = new List<TilesetBinding>();
			while (true)
			{
				var tilesetId = reader.ReadByte();
				if (tilesetId == 255)
					break;
				var name = reader.ReadCString();
				var version = reader.ReadUInt16();
				tilesets.Add(new TilesetBinding { Id = tilesetId, Name = name, Version = version });
			}
			ev.Tilesets = tilesets.ToArray();
			ev.Tiles = reader.ReadBytes(100 * 100);
			for (int i = 0; i < ev.Z.Length; i++)
				ev.Z[i] = reader.ReadInt16();
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				int fl = pfl[pidx];
				int type = reader.ReadByte();
				var c1 = new Point2D(reader.ReadByte(), reader.ReadByte());
				var c2 = new Point2D(reader.ReadByte(), reader.ReadByte());

				int ol;
				if (type == 0)
					ol = ((fl & 1) == 1) ? 2 : 1;
				else if (type == 1)
					ol = ((fl & 1) == 1) ? 8 : 4;
				else
					throw new Exception("Unknown plot type: " + type);

				for (int y = c1.Y; y <= c2.Y; y++)
					for (int x = c1.X; x <= c2.X; x++)
						ev.Overlays[y * 100 + x] |= ol;
			}
			return ev;
		}

		public static PartyUpdate ReadPartyUpdateEvent(this BinaryDataReader reader)
		{
			var ids = new List<int>();
			while (true)
			{
				int id = reader.ReadInt32();
				if (id == -1)
					break;
				ids.Add(id);
			}
			return new PartyUpdate { MemberIds = ids.ToArray() };
		}

		public static PartyChangeLeader ReadPartyLeaderChangeEvent(this BinaryDataReader reader)
		{
			return new PartyChangeLeader {
				LeaderId = reader.ReadInt32()
			};
		}

		public static PartyUpdateMember ReadPartyMemberUpdateEvent(this BinaryDataReader reader)
		{
			var memberId = reader.ReadInt32();
			var visible = reader.ReadByte() == 1;
			var location = visible ? reader.ReadInt32Coord() : (Point2D?)null;
			var color = reader.ReadColor();
			return new PartyUpdateMember {
				Color = color,
				Location = location,
				MemberId = memberId
			};
		}

		public static PlaySound ReadPlaySoundEvent(this BinaryDataReader reader)
		{
			return new PlaySound {
				ResourceId = reader.ReadUInt16(),
				Volume = reader.ReadUInt16() / 256.0,
				Speed = reader.ReadUInt16() / 256.0
			};
		}

		public static LoadResource ReadResourceLoadEvent(this BinaryDataReader reader)
		{
			return new LoadResource {
				ResourceId = reader.ReadUInt16(),
				Name = reader.ReadCString(),
				Version = reader.ReadUInt16()
			};
		}

		public static WidgetCreate ReadWidgetCreateEvent(this BinaryDataReader reader)
		{
			var id = reader.ReadUInt16();
			var type = reader.ReadCString();
			var parentId = reader.ReadUInt16();
			var pargs = reader.ReadList();
			var cargs = reader.ReadList();

			return new WidgetCreate {
				Id = id,
				Type = type,
				ParentId = parentId,
				CArgs = cargs,
				PArgs = pargs
			};
		}

		public static WidgetDestroy ReadWidgetDestroyEvent(this BinaryDataReader reader)
		{
			return new WidgetDestroy {
				WidgetId = reader.ReadUInt16()
			};
		}

		public static WidgetMessage ReadWidgetMessageEvent(this BinaryDataReader reader)
		{
			return new WidgetMessage {
				WidgetId = reader.ReadUInt16(),
				Name = reader.ReadCString(),
				Args = reader.ReadList()
			};
		}

		public static void ReadSessionKeyEvent(this BinaryDataReader reader)
		{
		}

		private static byte[] Unpack(byte[] input)
		{
			var buf = new byte[4096];
			var inflater = new Inflater();
			using (var output = new MemoryStream())
			{
				inflater.SetInput(input, 0, input.Length);
				int n;
				while ((n = inflater.Inflate(buf)) != 0)
					output.Write(buf, 0, n);

				if (!inflater.IsFinished)
					throw new Exception("Got unterminated map blob");

				return output.ToArray();
			}
		}
	}
}
