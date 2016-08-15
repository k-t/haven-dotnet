using System;
using System.Collections.Generic;
using System.Linq;

namespace Haven.Utils
{
	public class ValueTreeNode<T> : TreeNode<ValueTreeNode<T>>
	{
		private static readonly Func<T, T, bool> comparer = EqualityComparer<T>.Default.Equals;

		public ValueTreeNode() : this(default(T))
		{
		}

		public ValueTreeNode(T value)
		{
			Value = value;
		}

		public T Value
		{
			get;
			set;
		}

		public ValueTreeNode<T> Find(T value)
		{
			return Children.FirstOrDefault(x => comparer(x.Value, value));
		}
	}
}
