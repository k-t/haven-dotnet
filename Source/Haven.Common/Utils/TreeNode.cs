using System.Collections.Generic;
using System.Linq;

namespace Haven.Utils
{
	public class TreeNode<T> where T : TreeNode<T>
	{
		private T parent;
		private T prev;
		private T next;
		private T firstChild;
		private T lastChild;

		public T Parent
		{
			get { return parent; }
		}

		public T Next
		{
			get { return next; }
		}

		public T Previous
		{
			get { return prev; }
		}

		public T FirstChild
		{
			get { return firstChild; }
		}

		public T LastChild
		{
			get { return lastChild; }
		}

		public IEnumerable<T> Children
		{
			get
			{
				for (var child = firstChild; child != null; child = child.next)
					yield return child;
			}
		}

		public IEnumerable<T> Descendants
		{
			get
			{
				var stack = new Stack<T>(Children);
				while (stack.Any())
				{
					var node = stack.Pop();
					yield return node;
					foreach (var child in node.Children)
						stack.Push(child);
				}
			}
		}

		public bool HasChildren
		{
			get { return firstChild != null; }
		}

		/// <summary>
		/// Adds specified node as a child to this node.
		/// </summary>
		public T AddChild(T node)
		{
			node.Remove();

			node.parent = (T)this;

			if (this.firstChild == null)
				this.firstChild = node;

			if (this.lastChild != null)
			{
				node.prev = this.lastChild;
				this.lastChild.next = node;
				this.lastChild = node;
			}
			else
				this.lastChild = node;

			return (T)this;
		}

		/// <summary>
		/// Adds range of children nodes to this node.
		/// </summary>
		public T AddChildren(IEnumerable<T> children)
		{
			foreach (var child in children)
				AddChild(child);
			return (T)this;
		}

		/// <summary>
		/// Adds range of children nodes to this node.
		/// </summary>
		public T AddChildren(params T[] children)
		{
			return AddChildren(children.AsEnumerable());
		}

		/// <summary>
		/// Removes this node from the parent node.
		/// </summary>
		public void Remove()
		{
			if (parent != null)
			{
				if (parent.firstChild == this)
					parent.firstChild = next;
				if (parent.lastChild == this)
					parent.lastChild = prev;
			}
			if (next != null)
				next.prev = prev;
			if (prev != null)
				prev.next = next;
			prev = null;
			next = null;
			parent = null;
		}
	}
}
