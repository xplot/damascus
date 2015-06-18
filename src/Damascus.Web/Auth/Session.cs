using System;

namespace Damascus.Web
{
	public static class SessionStatus
	{
		public const string EXPIRED = "EXPIRED";
		public const string VALID = "VALID"; 
	}
	
	public class Session
	{
		public Guid Id { get; set; }
		public string Token { get; set; }
		public DateTime ExpiresOn { get; set; }
		public Guid UserId { get; set; }
		public string Status { get; set; }	
	}
}