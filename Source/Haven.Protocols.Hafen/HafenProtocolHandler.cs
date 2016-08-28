using System;
using System.Collections.Generic;
using Haven.Net;
using Haven.Protocols.Hafen.Messages;
using Haven.Protocols.Hafen.Utils;
using Haven.Utils;

namespace Haven.Protocols.Hafen
{
	public class HafenProtocolHandler : ProtocolHandlerBase
	{
		#region Constants

		private const int ProtocolVersion = 6;

		private const int MSG_SESS = 0;
		private const int MSG_MAPREQ = 4;

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
		[Obsolete] private const int RMSG_TILES = 11;
		[Obsolete] private const int RMSG_BUFF = 12;
		private const int RMSG_SESSKEY = 13;

		private const int GMSG_TIME = 0;
		[Obsolete] private const int GMSG_ASTRO = 1;
		private const int GMSG_LIGHT = 2;
		private const int GMSG_SKY = 3;
		private const int GMSG_WEATHER = 4;

		private const int PD_LIST = 0;
		private const int PD_LEADER = 1;
		private const int PD_MEMBER = 2;

		private const int OD_REM = 0;
		private const int OD_MOVE = 1;
		private const int OD_RES = 2;
		private const int OD_LINBEG = 3;
		private const int OD_LINSTEP = 4;
		private const int OD_SPEECH = 5;
		private const int OD_COMPOSE = 6;
		private const int OD_ZOFF = 7;
		private const int OD_LUMIN = 8;
		private const int OD_AVATAR = 9;
		private const int OD_FOLLOW = 10;
		private const int OD_HOMING = 11;
		private const int OD_OVERLAY = 12;
		[Obsolete] private const int OD_AUTH = 13;
		private const int OD_HEALTH = 14;
		private const int OD_BUDDY = 15;
		private const int OD_CMPPOSE = 16;
		private const int OD_CMPMOD = 17;
		private const int OD_CMPEQU = 18;
		private const int OD_ICON = 19;
		private const int OD_RESATTR = 20;
		private const int OD_END = 255;

		#endregion

		public enum GameClientState
		{
			Stopped = 0,
			Created = 1,
			Started = 2,
			Stopping = 3
		}

		private readonly Dictionary<int, FragmentBuffer> mapFrags;

		public HafenProtocolHandler()
		{
			mapFrags = new Dictionary<int, FragmentBuffer>();
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

		protected override BinaryMessage GetHelloMessage(string sessionId, byte[] cookie)
		{
			return BinaryMessage.Make(MSG_SESS)
				.UInt16(2)
				.String("Hafen")
				.UInt16(ProtocolVersion)
				.String(sessionId)
				.UInt16((ushort)cookie.Length)
				.Bytes(cookie)
				.List()
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
					int type = reader.ReadByte();
					switch (type)
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
					bool isIncremental = reader.ReadByte() != 0;
					while (reader.HasRemaining)
					{
						var type = reader.ReadCString();
						var args = reader.ReadList();
						switch (type)
						{
							case "tm":
								Receive(new UpdateGameTime {
									Time = (int)args[0],
									IsIncremental = isIncremental
								});
								break;
							case "astro":
								Receive(new UpdateAstronomy {
									Day = (float)args[0],
									MoonPhase = (float)args[1],
									Year = (float)args[2],
									IsNight = (byte)args[3] != 0,
									MoonColor = (Color)args[4]
								});
								break;
							case "light":
								Receive(new UpdateLighting {
									Ambient = (Color)args[0],
									Diffuse = (Color)args[1],
									Specular = (Color)args[2],
									Angle = (float)args[3],
									Elevation = (float)args[4]
								});
								break;
							case "sky":
								if (args.Length > 0)
									// TODO: add actual data to sky update message
									Receive(new SkyUpdate());
								else
									Receive(new SkyClear());
								break;
							case "wth":
								// TODO: add actual data to weather update message
								Receive(new UpdateWeather());
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
				case RMSG_SESSKEY:
					reader.ReadSessionKeyEvent();
					break;
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
				ev.Flags = (UpdateGameObjectFlags)reader.ReadByte();
				ev.GobId = reader.ReadUInt32();
				ev.Frame = reader.ReadInt32();
				while (true)
				{
					var delta = DecodeGobDelta(reader);
					if (delta == null)
						break;
					ev.Deltas.Add(delta);
				}
				Receive(ev);
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
				case OD_COMPOSE:
					return reader.ReadGobComposite();
				case OD_CMPPOSE:
					return reader.ReadGobCompositePose();
				case OD_CMPMOD:
					return reader.ReadGobCompositeModel();
				case OD_CMPEQU:
					return reader.ReadGobCompositeEqu();
				case OD_AVATAR:
					return reader.ReadGobAvatar();
				case OD_ZOFF:
					return reader.ReadGobZOffset();
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
				case OD_ICON:
					return reader.ReadGobIcon();
				case OD_RESATTR:
					return reader.ReadResAttr();
				case OD_END:
					return null;
				default:
					// TODO: MessageException
					throw new Exception("Unknown objdelta type: " + type);
			}
		}
	}
}
