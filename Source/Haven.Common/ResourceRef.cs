using System;

namespace Haven
{
	public struct ResourceRef : IEquatable<ResourceRef>
	{
		public ResourceRef(string name, ushort version)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			Name = name;
			Version = version;
		}

		public string Name { get; }

		public ushort Version { get; }

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Version.GetHashCode();
		}

		public bool Equals(ResourceRef other)
		{
			return string.Equals(Name, other.Name) && Version == other.Version;
		}

		public override bool Equals(object obj)
		{
			return (obj is ResourceRef) && Equals((ResourceRef)obj);
		}

		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(Name);
		}
	}
}
