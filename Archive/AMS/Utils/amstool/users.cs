using System;
using System.Collections;
using System.Data.SqlClient;

namespace XMAdmin
{
	/// <summary>
	/// Represents a single user of the AMS system.
	/// </summary>
	public class User
	{
		//Data fields
		public readonly	XMGuid	UserId;
		public			XMGuid	AccessToken;
		public			string	HostIp;
		public			string	Login;
		public			string	Password;
		public			bool	Enabled;
		public			bool	Online;
		public			int		Datarate;
		public			string	Email;
		public			int		Age;
		public			int		Gender;
		public			int		SexPref;
		public			string	Browser;
		public			bool	Paying;
		public			bool	Subscribed;

		/// <summary>
		/// Load data concerning a user.
		/// </summary>
		/// <param name="newUserId">UserID of the user the load.</param>
		public User(XMGuid newUserId)
		{
			//save the userid
			UserId = newUserId;

			//load data on the user
			SqlDataReader rs = Data.Exec(String.Format(
				"select * from users where userid={0}", newUserId.ToStringDB()));
			if (!rs.Read())
				throw new Exception("User not found.");
			
			//copy data into local vars
			FromDb(rs);
			rs.Close();
		}

		/// <summary>
		/// Load data from the given recordset.
		/// </summary>
		/// <param name="rs">SqlDataReader containing all
		/// fields from the Users table</param>
		public User(SqlDataReader rs)
		{
			//read the userid first, since it has to happen
			//in a constructor
			UserId = new XMGuid((byte[])rs["UserID"]);

			//read data from the fields of the recordset
			FromDb(rs);
		}

		/// <summary>
		/// Load data from the given recordset.
		/// </summary>
		/// <param name="rs">SqlDataReader containing all
		/// fields from the Users table</param>
		private void FromDb(SqlDataReader rs)
		{
			if (rs["accesstoken"] != DBNull.Value)
				AccessToken = new XMGuid((byte[])rs["accesstoken"]);
			Login = Convert.ToString(rs["login"]);
			Password = Convert.ToString(rs["password"]);
			Enabled = Convert.ToBoolean(rs["enabled"]);
			Online = Convert.ToBoolean(rs["online"]);
			HostIp = Convert.ToString(rs["hostip"]);
			Datarate = Convert.ToInt32(rs["datarate"]);
			if (rs["email"] != DBNull.Value)
				Email = Convert.ToString(rs["email"]);
			if (rs["age"] != DBNull.Value)
				Age = Convert.ToInt32(rs["age"]);
			if (rs["gender"] != DBNull.Value)
				Gender = Convert.ToInt32(rs["gender"]);
			if (rs["sexpref"] != DBNull.Value)
				SexPref = Convert.ToInt32(rs["sexpref"]);
			if (rs["browser"] != DBNull.Value)
				Browser = Convert.ToString(rs["browser"]);
			Paying = Convert.ToBoolean(rs["paying"]);
			Subscribed = Convert.ToBoolean(rs["subsribe"]);
		}

		public Indexes Indexes
		{
			get
			{
				return Indexes.FromUser(UserId);
			}
		}
	}

	/// <summary>
	/// Collection of users. Use static functions to query
	/// for users--do not create instances directly.
	/// </summary>
	public class Users : ReadOnlyCollectionBase
	{
		// ---------------------------------------------- SEARCH METHODS

		/// <summary>
		/// Enumerate every user in the system.
		/// </summary>
		/// <returns>Users instance containing all users in the system.</returns>
		public static Users FindAllUsers()
		{
			return new Users("select * from users");
		}

		/// <summary>
		/// Enumerate users with a login field matching the supplied paramter.
		/// </summary>
		/// <param name="login">Login to search for.</param>
		/// <returns>Users instance containing all users that match the criterea.</returns>
		public static Users FindByLogin(string login)
		{
			return new Users(String.Format(
				"select * from users where login like '%{0}%'", login.Replace("'", "''")));
		}

		/// <summary>
		/// Enumerate users that are currently active in the system.
		/// </summary>
		/// <returns>Users instance containing all users that match the criterea.</returns>
		public static Users FindByOnline()
		{
			return new Users("select * from users where not accesstoken is null");
		}

		// ---------------------------------------------- IMPLEMENTATION

		/// <summary>
		/// Create a new users collection.
		/// </summary>
		/// <param name="sql">Query used to generate results.</param>
		private Users(string sql)
		{
			//constructor is private.. use static function
			//to create instances

			//get all users that match the criterea
			SqlDataReader rs = Data.Exec(sql);

			//add users to internal collection
			while (rs.Read())
			{
				InnerList.Add(new User(rs));	
			}
			rs.Close();
		}
	}
}
