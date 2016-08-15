using System;

namespace Haven
{
	public struct ResourceRef
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

		public override bool Equals(object obj)
		{
			if (!(obj is ResourceRef))
				return false;
			var other = (ResourceRef)obj;
			return string.Equals(Name, other.Name) && Version == other.Version;
		}

		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(Name);
		}
	}
}
