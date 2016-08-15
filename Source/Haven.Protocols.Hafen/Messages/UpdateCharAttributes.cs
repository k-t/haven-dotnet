namespace Haven.Protocols.Hafen.Messages
{
	public class UpdateCharAttributes
	{
		public CharAttribute[] Attributes
		{
			get;
			set;
		}
	}

	public class CharAttribute
	{
		public string Name
		{
			get;
			set;
		}

		public int BaseValue
		{
			get;
			set;
		}

		public int ModifiedValue
		{
			get;
			set;
		}
	}
}
