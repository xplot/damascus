using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Damascus.Web
{
	public class AuthenticationStore: IDisposable
	{
		public SqlConnection Connection { get ; set ; }
		public string user_tableName = "User";
		public string session_tableName = "Session";

		public User GetUser (Guid id)
		{
			var queryString = "SELECT * FROM " + user_tableName + " WHERE id = @id";

			SqlCommand command = new SqlCommand(queryString, Connection);
			command.Parameters.AddWithValue("@id", id);
			
			SqlDataReader reader = command.ExecuteReader();
			if(!reader.Read ())
				return null;

			return GetUserFromReader(reader);
		}
		
		public User GetUserFromUsername (string username)
		{
			var queryString = "SELECT * FROM " + user_tableName + " WHERE username = @username";

			SqlCommand command = new SqlCommand(queryString, Connection);
			command.Parameters.AddWithValue("@username", username);
			
			SqlDataReader reader = command.ExecuteReader();
			if(!reader.Read ())
				return null;

			return GetUserFromReader(reader);
		}
		
		public Session GetSessionFromToken (string token)
		{
			var queryString = "SELECT * FROM " + session_tableName + " WHERE token = @token";

			SqlCommand command = new SqlCommand(queryString, Connection);
			command.Parameters.AddWithValue("@token", token);
			
			SqlDataReader reader = command.ExecuteReader();
			if(!reader.Read ())
				return null;

			return new Session{
				Id = reader.GetGuid(0),
				UserId = reader.GetGuid(1),
				Token = reader.GetString(2),
				ExpiresOn = reader.GetDateTime(3),
				Status = reader.GetString(4),
			};
		}

		public Guid CreateUser (User user)
		{
			if(user.Id != Guid.Empty)
				user.Id = Guid.NewGuid();
				
			var queryString = "INSERT INTO " + user_tableName + " (id,name,last_name,username,password,super_user) VALUES (@id, @name, @lastName,@username,@password, @superUser)";
			SqlCommand command = new SqlCommand (queryString, this.Connection);	

			command.Parameters.AddWithValue ("@id", user.Id);
			command.Parameters.AddWithValue ("@name", user.Name);
			command.Parameters.AddWithValue ("@lastName", user.LastName);
			command.Parameters.AddWithValue ("@username", user.Username);
			command.Parameters.AddWithValue ("@password", user.Password);
			command.Parameters.AddWithValue ("@superUser", user.SuperUser);

			command.ExecuteNonQuery ();
			
			return user.Id;
		}

		public Guid CreateSession (Session session)
		{
			if(session.Id != Guid.Empty)
				session.Id = Guid.NewGuid();
				
			var queryString = "INSERT INTO " + session_tableName + " (id,user_id,token,expires_on,status) VALUES (@id,@userId, @token, @expiresOn, @status)";
			SqlCommand command = new SqlCommand (queryString, this.Connection);	

			command.Parameters.AddWithValue ("@id", session.Id);
			command.Parameters.AddWithValue ("@userId", session.UserId);
			command.Parameters.AddWithValue ("@token", session.Token);
			command.Parameters.AddWithValue ("@expiresOn", session.ExpiresOn);
			command.Parameters.AddWithValue ("@status", session.Status);

			command.ExecuteNonQuery ();
			
			return session.Id;
		}
		
		public void InvalidateAllUserSessions (User user)
		{
			var queryString = "UPDATE " + session_tableName + "SET status=@status where user_id=@id";
			SqlCommand command = new SqlCommand (queryString, this.Connection);	

			command.Parameters.AddWithValue ("@id", user.Id);
			command.Parameters.AddWithValue ("@status", SessionStatus.EXPIRED);
			
			command.ExecuteNonQuery ();
		}
		
		private User GetUserFromReader(SqlDataReader reader){
			return new User{
				Id = reader.GetGuid(0),
				Name = reader.GetString(1),
				LastName = reader.GetString(2),
				Username = reader.GetString(3),
				Password = reader.GetString(4),
				SuperUser = reader.GetBoolean(5),
			};
		}
		
		bool disposed = false;
		public void Dispose()
		{
			if(disposed)
				return;
				
			this.Connection.Dispose();
			disposed = true;
		}
	}
}

