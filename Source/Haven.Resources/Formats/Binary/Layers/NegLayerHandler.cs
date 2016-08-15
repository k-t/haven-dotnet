using System.Linq;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class NegLayerHandler : GenericLayerHandler<NegLayer>
	{
		private const int DirectionCount = 8;

		public NegLayerHandler() : base("neg")
		{
		}

		protected override NegLayer Deserialize(BinaryDataReader reader)
		{
			var neg = new NegLayer();
			neg.Center = reader.ReadInt16Coord();
			neg.Hitbox = Rect.FromLTRB(reader.ReadInt16Coord(), reader.ReadInt16Coord());
			// not sure what that data means but preserve it anyway
			neg.Sz = reader.ReadInt16Coord();
			var en = reader.ReadByte(); /* number of E? */
			neg.Ep = new Point2D[DirectionCount][];
			for (int i = 0; i < en; i++)
			{
				var epid = reader.ReadByte();
				var cnt = reader.ReadUInt16();
				neg.Ep[epid] = new Point2D[cnt];
				for (int j = 0; j < cnt; j++)
					neg.Ep[epid][j] = reader.ReadInt16Coord();
			}
			return neg;
		}

		protected override void Serialize(BinaryDataWriter writer, NegLayer neg)
		{
			writer.WriteInt16Coord(neg.Center);
			writer.WriteInt16Coord(neg.Hitbox.Left, neg.Hitbox.Top);
			writer.WriteInt16Coord(neg.Hitbox.Right, neg.Hitbox.Bottom);
			writer.WriteInt16Coord(neg.Sz);
			writer.Write((byte)neg.Ep.Count(x => x != null));
			for (int i = 0; i < neg.Ep.Length; i++)
			{
				if (neg.Ep[i] == null)
					continue;
				writer.Write((byte)i);
				writer.Write((ushort)neg.Ep[i].Length);
				for (int j = 0; j < neg.Ep[i].Length; j++)
					writer.WriteInt16Coord(neg.Ep[i][j]);
			}
		}
	}
}