using System.Collections.Generic;
using System.Linq;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class PoseLayerHandler : GenericLayerHandler<PoseLayer>
	{
		public PoseLayerHandler() : base("skan")
		{
		}

		protected override PoseLayer Deserialize(BinaryDataReader reader)
		{
			var pose = new PoseLayer();
			pose.Id = reader.ReadInt16();
			pose.Flags = reader.ReadByte();
			pose.Mode = reader.ReadByte();
			pose.Length = reader.ReadFloat40();
			if ((pose.Flags & 1) != 0)
				pose.Speed = reader.ReadFloat40();
			var effects = new List<PoseEffect>();
			var tracks = new List<PoseTrack>();
			while (reader.HasRemaining)
			{
				var boneName = reader.ReadCString();
				if (boneName == "{ctl}")
					effects.Add(ParseEffect(reader));
				else
					tracks.Add(new PoseTrack { BoneName = boneName, Frames = ParseFrames(reader) });
			}
			pose.Effects = effects.ToArray();
			pose.Tracks = tracks.ToArray();
			return pose;
		}

		protected override void Serialize(BinaryDataWriter writer, PoseLayer pose)
		{
			writer.Write(pose.Id);
			writer.Write(pose.Flags);
			writer.Write(pose.Mode);
			writer.WriteFloat40(pose.Length);
			if ((pose.Flags & 1) != 0)
				writer.WriteFloat40(pose.Speed);
			foreach (var effect in pose.Effects)
			{
				writer.WriteCString("{ctl}");
				writer.Write((ushort)effect.Events.Length);
				foreach (var ev in effect.Events)
				{
					writer.WriteFloat40(ev.Time);
					writer.Write(ev.Type);
					switch (ev.Type)
					{
						case 0:
							writer.WriteCString(ev.ResRef.Name);
							writer.Write(ev.ResRef.Version);
							writer.Write((byte)ev.Data.Length);
							writer.Write(ev.Data);
							break;
						case 1:
							writer.WriteCString(ev.Id);
							break;
					}
				}
			}
			foreach (var track in pose.Tracks)
			{
				writer.WriteCString(track.BoneName);
				writer.Write((ushort)track.Frames.Length);
				foreach (var frame in track.Frames)
				{
					writer.WriteFloat40(frame.Time);
					for (int o = 0; o < 3; o++)
						writer.WriteFloat40(frame.Translation[o]);
					writer.WriteFloat40(frame.RotationAngle);
					for (int o = 0; o < 3; o++)
						writer.WriteFloat40(frame.RotationAxis[o]);
				}
			}
		}

		private PoseFrame[] ParseFrames(BinaryDataReader reader)
		{
			var count = reader.ReadUInt16();
			var frames = new PoseFrame[count];
			for (int i = 0; i < frames.Length; i++)
			{
				var frame = new PoseFrame();
				frame.Time = reader.ReadFloat40();
				frame.Translation = new double[3];
				for (int o = 0; o < 3; o++)
					frame.Translation[o] = reader.ReadFloat40();
				frame.RotationAngle = reader.ReadFloat40();
				frame.RotationAxis = new double[3];
				for (int o = 0; o < 3; o++)
					frame.RotationAxis[o] = reader.ReadFloat40();
				frames[i] = frame;
			}
			return (frames);
		}

		private PoseEffect ParseEffect(BinaryDataReader reader)
		{
			var count = reader.ReadUInt16();
			var events = new PoseEvent[count];
			for (int i = 0; i < events.Length; i++)
			{
				var ev = new PoseEvent();
				ev.Time = reader.ReadFloat40();
				ev.Type = reader.ReadByte();
				switch (ev.Type)
				{
					case 0:
						var resnm = reader.ReadCString();
						var resver = reader.ReadUInt16();
						ev.ResRef = new ResourceRef(resnm, resver);
						ev.Data = reader.ReadBytes(reader.ReadByte());
						break;
					case 1:
						ev.Id = reader.ReadCString();
						break;
					default:
						throw new ResourceException("Illegal control event: " + ev.Type);
				}
				events[i] = ev;
			}
			return new PoseEffect {Events = events.ToArray()};
		}
	}
}
