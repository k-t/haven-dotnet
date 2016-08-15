using System;
using System.Collections.Generic;
using Haven.Net;
using Haven.Protocols.Legacy.Messages;
using Haven.Protocols.Legacy.Utils;
using Haven.Utils;

namespace Haven.Protocols.Legacy
{
	public class LegacyProtocolHandler : ProtocolHandlerBase
	{
		#region Constants

		private const int ProtocolVersion = 2;

		internal const int MSG_SESS = 0;
		internal const int MSG_CLOSE = 8;
		internal const int MSG_MAPREQ = 4;

		private const int RMSG_NEWWDG = 0;
		private const int RMSG_WDGMSG = 1;
		private const int RMSG_DSTWDG = 2;
		private const int RMSG_MAPIV = 3;
		private const int RMSG_GLOBLOB = 4;
		private const int RMSG_PAGINAE = 5;
		private const int RMSG_RESID = 6;
		private const int RMSG_PARTY = 7;
		private const int RMSG_SFX = 8;
		private const int RMSG_CATTR = 9;
		private const int RMSG_MUSIC = 10;
		private const int RMSG_TILES = 11;
		private const int RMSG_BUFF = 12;

		private const int GMSG_TIME = 0;
		private const int GMSG_ASTRO = 1;
		private const int GMSG_LIGHT = 2;

		private const int OD_REM = 0;
		private const int OD_MOVE = 1;
		private const int OD_RES = 2;
		private const int OD_LINBEG = 3;
		private const int OD_LINSTEP = 4;
		private const int OD_SPEECH = 5;
		private const int OD_LAYERS = 6;
		private const int OD_DRAWOFF = 7;
		private const int OD_LUMIN = 8;
		private const int OD_AVATAR = 9;
		private const int OD_FOLLOW = 10;
		private const int OD_HOMING = 11;
		private const int OD_OVERLAY = 12;
		/* private const int OD_AUTH = 13; -- Removed */
		private const int OD_HEALTH = 14;
		private const int OD_BUDDY = 15;
		private const int OD_END = 255;

		private const int PD_LIST = 0;
		private const int PD_LEADER = 1;
		private const int PD_MEMBER = 2;

		#endregion

		private readonly Dictionary<int, FragmentBuffer> mapFrags;

		public LegacyProtocolHandler()
		{
			this.mapFrags = new Dictionary<int, FragmentBuffer>();
		}

		public override void Send<TMessage>(TMessage message)
		{
			var gridRequest = message as MapRequestGrid;
			if (gridRequest != null)
			{
				var msg = BinaryMessage.Make(MSG_MAPREQ)
					.Coord(gridRequest.Coord)
					.Complete();
				Send(msg);
				return;
			}

			var widgetMessage = message as WidgetMessage;
			if (widgetMessage != null)
			{
				var msg = BinaryMessage.Make(RMSG_WDGMSG)
					.UInt16(widgetMessage.WidgetId)
					.String(widgetMessage.Name);

				if (widgetMessage.Args != null)
					msg.List(widgetMessage.Args);

				SendSeqMessage(msg.Complete());
				return;
			}

			throw new ArgumentException($"Unsupported outgoing message type '{message.GetType().Name}'");
		}

		protected override BinaryMessage GetHelloMessage(string userName, byte[] cookie)
		{
			return BinaryMessage.Make(MSG_SESS)
				.UInt16(1)
				.String("Haven")
				.UInt16(ProtocolVersion)
				.String(userName)
				.Bytes(cookie)
				.Complete();
		}

		protected override void HandleSeqMessage(BinaryMessage message)
		{
			var reader = message.GetReader();
			switch (message.Type)
			{
				case RMSG_NEWWDG:
					Receive(reader.ReadWidgetCreateEvent());
					break;
				case RMSG_WDGMSG:
					Receive(reader.ReadWidgetMessageEvent());
					break;
				case RMSG_DSTWDG:
					Receive(reader.ReadWidgetDestroyEvent());
					break;
				case RMSG_MAPIV:
				{
					int invalidationType = reader.ReadByte();
					switch (invalidationType)
					{
						case 0:
							Receive(reader.ReadMapInvalidateGridEvent());
							break;
						case 1:
							Receive(reader.ReadMapInvalidateRegionEvent());
							break;
						case 2:
							Receive(new MapInvalidate());
							break;
					}
					break;
				}
				case RMSG_GLOBLOB:
					while (reader.HasRemaining)
					{
						switch (reader.ReadByte())
						{
							case GMSG_TIME:
								Receive(reader.ReadGameTimeUpdateEvent());
								break;
							case GMSG_ASTRO:
								Receive(reader.ReadAstronomyUpdateEvent());
								break;
							case GMSG_LIGHT:
								Receive(reader.ReadAmbientLightUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_PAGINAE:
					Receive(reader.ReadGameActionsUpdateEvent());
					break;
				case RMSG_RESID:
					Receive(reader.ReadResourceLoadEvent());
					break;
				case RMSG_PARTY:
					while (reader.HasRemaining)
					{
						int type = reader.ReadByte();
						switch (type)
						{
							case PD_LIST:
								Receive(reader.ReadPartyUpdateEvent());
								break;
							case PD_LEADER:
								Receive(reader.ReadPartyLeaderChangeEvent());
								break;
							case PD_MEMBER:
								Receive(reader.ReadPartyMemberUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_SFX:
					Receive(reader.ReadPlaySoundEvent());
					break;
				case RMSG_CATTR:
					Receive(reader.ReadCharAttributesUpdateEvent());
					break;
				case RMSG_MUSIC:
					Receive(new PlayMusic());
					break;
				case RMSG_TILES:
					Receive(reader.ReadTilesetsLoadEvent());
					break;
				case RMSG_BUFF:
				{
					var type = reader.ReadCString();
					switch (type)
					{
						case "clear":
							Receive(new BuffClearAll());
							break;
						case "rm":
							Receive(reader.ReadBuffRemoveEvent());
							break;
						case "set":
							Receive(reader.ReadBuffUpdateEvent());
							break;
					}
					break;
				}
				default:
					throw new Exception("Unknown rmsg type: " + message.Type);
			}
		}

		// TODO: delete lingering fragments?
		protected override void HandleMapData(BinaryDataReader reader)
		{
			int packetId = reader.ReadInt32();
			int offset = reader.ReadUInt16();
			int length = reader.ReadUInt16();

			FragmentBuffer fragbuf;
			if (mapFrags.TryGetValue(packetId, out fragbuf))
			{
				fragbuf.Add(offset, reader.ReadRemaining());
				if (fragbuf.IsComplete)
				{
					mapFrags.Remove(packetId);
					var fragReader = new BinaryDataReader(fragbuf.Content);
					Receive(fragReader.ReadMapUpdateEvent());
				}
			}
			else if (offset != 0 || reader.Length - 8 < length)
			{
				fragbuf = new FragmentBuffer(length);
				fragbuf.Add(offset, reader.ReadRemaining());
				mapFrags[packetId] = fragbuf;
			}
			else
			{
				Receive(reader.ReadMapUpdateEvent());
			}
		}

		protected override void HandleGobData(BinaryDataReader reader)
		{
			while (reader.HasRemaining)
			{
				var ev = new UpdateGameObject();
				ev.ReplaceFlag = (reader.ReadByte() & 1) != 0;
				ev.GobId = reader.ReadInt32();
				ev.Frame = reader.ReadInt32();

				while (true)
				{
					var delta = DecodeGobDelta(reader);
					if (delta == null)
						break;
					ev.Deltas.Add(delta);
				}

				Receive(ev);
				AckGobData(ev.GobId, ev.Frame);
			}
		}

		private GobDelta DecodeGobDelta(BinaryDataReader reader)
		{
			int type = reader.ReadByte();
			switch (type)
			{
				case OD_REM:
					return new GobDelta.Clear();
				case OD_MOVE:
					return reader.ReadGobPosition();
				case OD_RES:
					return reader.ReadGobResource();
				case OD_LINBEG:
					return reader.ReadGobStartMovement();
				case OD_LINSTEP:
					return reader.ReadGobAdjustMovement();
				case OD_SPEECH:
					return reader.ReadGobSpeech();
				case OD_LAYERS:
					return reader.ReadGobLayers();
				case OD_AVATAR:
					return reader.ReadGobAvatar();
				case OD_DRAWOFF:
					return reader.ReadGobDrawOffset();
				case OD_LUMIN:
					return reader.ReadGobLight();
				case OD_FOLLOW:
					return reader.ReadGobFollow();
				case OD_HOMING:
					return reader.ReadGobHoming();
				case OD_OVERLAY:
					return reader.ReadGobOverlay();
				case OD_HEALTH:
					return reader.ReadGobHealth();
				case OD_BUDDY:
					return reader.ReadGobBuddy();
				case OD_END:
					return null;
				default:
					// TODO: MessageException
					throw new Exception("Unknown objdelta type: " + type);
			}
		}
	}
}
