namespace Haven.Net
{
	public class AuthResult
	{
		private readonly bool isSuccessful;
		private readonly string sessionId;
		private readonly byte[] sessionCookie;
		private readonly byte[] sessionToken;

		public AuthResult(bool isSuccessful, string sessionId, byte[] sessionCookie, byte[] sessionToken = null)
		{
			this.isSuccessful = isSuccessful;
			this.sessionId = sessionId;
			this.sessionCookie = sessionCookie;
			this.sessionToken = sessionToken;
		}

		public bool IsSuccessful
		{
			get { return isSuccessful; }
		}

		public string SessionId
		{
			get { return sessionId; }
		}

		public byte[] SessionCookie
		{
			get { return sessionCookie; }
		}

		public byte[] SessionToken
		{
			get { return sessionToken; }
		}

		public static AuthResult Success(string sessionId, byte[] sessionCookie, byte[] sessionToken = null)
		{
			return new AuthResult(true, sessionId, sessionCookie, sessionToken);
		}

		public static AuthResult Fail()
		{
			return new AuthResult(false, null, null);
		}
	}
}
