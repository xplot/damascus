using System;

namespace Damascus.Web
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool SuperUser { get; set; }	
	}
}