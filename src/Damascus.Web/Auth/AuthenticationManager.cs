using System;
using BCrypt.Net;

namespace Damascus.Web
{
	public class AuthenticationManager
	{
		public AuthenticationStore AuthenticationStore { get; set; }
		
		public AuthenticationManager(AuthenticationStore authStore)
		{
			this.AuthenticationStore = authStore;
		}
		
		public void Register(User user)
		{
			user.Password = Encrypt(user.Password);
			AuthenticationStore.CreateUser(user);
		}
		
		public Session Validate(string token)
		{
			var session = AuthenticationStore.GetSessionFromToken(token);
			return (session != null && session.ExpiresOn > DateTime.Now) ? session: null;	
		}
		
		public Session Authenticate(string username, string password)
		{
			var user = AuthenticationStore.GetUserFromUsername(username);
			
			if(user == null)
				return null;
			
			if(!CheckPassword(password, user.Password))
				return null;
			
			var currentSession = AuthenticationStore.GetUserActiveSession(user);
			if(currentSession != null)
				return currentSession; 
			
			//Ensure there's only one session active per user
			AuthenticationStore.InvalidateAllUserSessions(user);
			
			var session = new Session()
			{
				Token = Guid.NewGuid().ToString(),
				ExpiresOn = GetDurationFromUser(user),
				UserId = user.Id,
				Status = SessionStatus.VALID
			};
			
			AuthenticationStore.CreateSession(session);
			
			return session;	
		}
		
		private DateTime GetDurationFromUser(User user)
		{
			if(user.SuperUser)
				return DateTime.Now.AddDays(1000); 
			
			return DateTime.Now.AddMinutes(60);
		}
		
		private string Encrypt(string password)
		{
			// Pass a logRounds parameter to GenerateSalt to explicitly specify the
			// amount of resources required to check the password. The work factor
			// increases exponentially, so each increment is twice as much work. If
			// omitted, a default of 10 is used.
			return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
		}
		
		private bool CheckPassword(string password, string hash)
		{
			return BCrypt.Net.BCrypt.Verify(password, hash);
		}
	}
}