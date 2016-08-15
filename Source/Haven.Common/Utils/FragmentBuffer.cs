using System;
using System.Collections.Generic;

namespace Haven.Utils
{
	public class FragmentBuffer
	{
		private readonly byte[] content;
		private readonly LinkedList<Hole> holes;

		public FragmentBuffer(int length)
		{
			content = new byte[length];
			holes = new LinkedList<Hole>();
			holes.AddFirst(new Hole(0, length));
		}

		public byte[] Content
		{
			get { return content; }
		}

		public bool IsComplete
		{
			get { return holes.Count == 0; }
		}

		public void Add(int offset, byte[] frag)
		{
			Add(offset, frag, 0, frag.Length);
		}

		public void Add(int offset, byte[] frag, int index, int count)
		{
			Array.Copy(frag, index, content, offset, count);
			int first = offset;
			int last = offset + count;
			for (var hole = holes.First; hole != null; hole = hole.Next)
			{
				if (first > hole.Value.Last || last < hole.Value.First)
					continue;
				if (first > hole.Value.First)
					holes.AddBefore(hole, new Hole(hole.Value.First, first - 1));
				if (last < hole.Value.Last)
					holes.AddAfter(hole, new Hole(last + 1, hole.Value.Last));
				holes.Remove(hole);
				break;
			}
		}

		private struct Hole
		{
			public readonly int First;
			public readonly int Last;

			public Hole(int first, int last)
			{
				First = first;
				Last = last;
			}
		}
	}
}
