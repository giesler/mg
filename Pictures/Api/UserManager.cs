using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace msn2.net.Pictures
{
	/// <summary>
	/// Summary description for UserManager.
	/// </summary>
	public class UserManager
	{
		private PicContext context;

		public UserManager(PicContext context)
		{
            this.context = context;
		}

		/// <summary>
		/// Adds a new user request to the database.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns>ID of request</returns>
		public int AddNewUserRequest(string name, string email, string password)
		{
			// add new login request
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd   = new SqlCommand("sp_LoginRequest_Add", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;

			// add params
			cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@password", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@id", SqlDbType.Int);
			cmd.Parameters["@id"].Direction = ParameterDirection.Output;

			cmd.Parameters["@name"].Value = name;
			cmd.Parameters["@email"].Value = email;
			cmd.Parameters["@password"].Value = password;

			// open connection and execute
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (int) cmd.Parameters["@id"].Value;
		}


		/// <summary>
		/// Returns a new ID to allow reseting the password
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public Guid GetPasswordResetKey(string email)
		{
			// connect to db and see if email is found
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd   = new SqlCommand("sp_ForgotPassword", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier);
			cmd.Parameters["@guid"].Direction = ParameterDirection.Output;

			cmd.Parameters["@Email"].Value = email;

			// run command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			if (cmd.Parameters["@guid"].Value == DBNull.Value)
			{
				return Guid.Empty;
			}

			return (Guid) cmd.Parameters["@guid"].Value;
		}


		public PersonInfo GetPerson(int personId)
		{

			// set up a connection and command to retreive info
			SqlConnection cn	= new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("dbo.sp_PersonInfo", cn);
			SqlDataReader dr	= null;
			PersonInfo info		= null;
			cmd.CommandType		= CommandType.StoredProcedure;
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);
			cmd.Parameters["@PersonID"].Value = personId;

			try 
			{
				cn.Open();
				dr		= cmd.ExecuteReader(CommandBehavior.SingleRow);
				info	= LoadPersonInfo(dr);
			}
			catch (SqlException excep) 
			{
				System.Diagnostics.Trace.Write(excep.ToString());
			}
			finally 
			{
				if (dr != null)
				{
					dr.Close();
				}
				// make sure connection is closed
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}

			return info;

		}

        public Person GetPersonA(int id)
        {
            Person p = (from d in PicContext.Current.DataContext.Persons
                        where d.PersonID == id
                        select d).First<Person>();
            return p;
        }

		public PersonInfo GetPerson(string userName)
		{
			// set up a connection and command to retreive info
			SqlConnection cn	= new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("dbo.sp_PersonInfoByUserName", cn);
			SqlDataReader dr	= null;
			PersonInfo info		= null;
			cmd.CommandType		= CommandType.StoredProcedure;
			cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 50);
			cmd.Parameters["@userName"].Value = userName;

			try 
			{
				cn.Open();
				dr		= cmd.ExecuteReader(CommandBehavior.SingleRow);
				info	= LoadPersonInfo(dr);
			}
			catch (SqlException excep) 
			{
				System.Diagnostics.Trace.Write(excep.ToString());
			}
			finally 
			{
				if (dr != null)
				{
					dr.Close();
				}
				// make sure connection is closed
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}

			return info;

		}

        public PersonInfo GetPersonByEmail(string email)
        {
            // set up a connection and command to retreive info
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_PersonInfoByEmail", cn);
            SqlDataReader dr = null;
            PersonInfo info = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
            cmd.Parameters["@email"].Value = email;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                info = LoadPersonInfo(dr);
            }
            catch (SqlException excep)
            {
                System.Diagnostics.Trace.Write(excep.ToString());
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                // make sure connection is closed
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }

            return info;

        }

		public PersonInfo Login(string email, string password, ref bool isValidEmail)
		{
			Trace.WriteLine("PersonInfo", "Starting");

			// set up a connection and command to retreive info
			
            SqlConnection cn	= new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("dbo.sp_Login2", cn);
			SqlDataReader dr	= null;
			PersonInfo info		= null;
			cmd.CommandType		= CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@ValidEmail", SqlDbType.Bit);
            cmd.Parameters.Add("@correctPassword", SqlDbType.NVarChar, 100);
            cmd.Parameters["@correctPassword"].Direction = ParameterDirection.Output;
			cmd.Parameters["@ValidEmail"].Direction	= ParameterDirection.Output;
			cmd.Parameters["@Email"].Value	  = email;
			cmd.Parameters["@Password"].Value = password;

			try 
			{
				cn.Open();
				dr		= cmd.ExecuteReader();
				info	= LoadPersonInfo(dr);

			}
			catch (SqlException excep) 
			{
				Trace.WriteLine("PersonInfo Constructor", excep.ToString());
			}
			finally 
			{
				if (dr != null)
				{
					dr.Close();
				}

				// make sure connection is closed
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}

            // Find out if we had a valid email
            isValidEmail = Convert.ToBoolean(cmd.Parameters["@ValidEmail"].Value);
            if (isValidEmail)
            {
                string correctPassword = cmd.Parameters["@correctPassword"].Value.ToString();
                
                // Check if at least half the characters are correct
                int correctCount = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    char char1 = password[i];
                    char char2 = '\0';
                    if (correctPassword.Length > i)
                    {
                        char2 = correctPassword[i];
                    }

                    if (char1 == char2)
                    {
                        correctCount++;
                    }
                }

                if (correctCount >= 7)
                {
                    info = this.GetPersonByEmail(email);
                }
            }

			return info;
		}

		private PersonInfo LoadPersonInfo(SqlDataReader dr)
		{
			// attempt to read
			if (dr.Read()) 
			{
				int id		= Convert.ToInt32(dr["PersonID"]);

				PersonInfo info = new PersonInfo(id, dr["FullName"].ToString(), dr["Email"].ToString());
				return info;
			} 

			return null;
			
		}

        public IQueryable<Person> GetPeople()
        {
            return this.context.DataContext.Persons;
        }

		public PersonInfo GetNewUserRequest(int id)
		{
			// set up objects to get info
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd   = new SqlCommand("sp_LoginRequest_Retreive", cn);
			SqlDataReader dr = null;
			PersonInfo info	 = null;
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@id", SqlDbType.Int);
			cmd.Parameters["@id"].Value = id;

			try
			{
				// retreive info on this id
				cn.Open();
				dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

				// attempt to read record
				if (dr.Read()) 
				{
					info = new PersonInfo(0, dr["Name"].ToString(), dr["Email"].ToString());
				}
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (dr != null)
				{
					dr.Close();
				}
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

			return info;

		}

		public void AssociateRequestWithPerson(int requestId, int personId)
		{
			// create conneciton, command
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_LoginRequest_Associate", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@RequestID", SqlDbType.Int);
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);

			cmd.Parameters["@RequestID"].Value = requestId;
			cmd.Parameters["@PersonID"].Value = personId;

			// run command
			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}
		}

		public void AddNewPerson(int requestId, string firstName, string lastName, string fullName)
		{
			// set up connection, command
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("sp_LoginRequest_NewPerson", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;

			// Add params to send to SP
			cmd.Parameters.Add("@RequestID", SqlDbType.Int);
			cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 50);

			cmd.Parameters["@requestId"].Value = requestId;
			cmd.Parameters["@FirstName"].Value = firstName;
			cmd.Parameters["@LastName"].Value = lastName;
			cmd.Parameters["@FullName"].Value = fullName;

			// run sp
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

        public void AddPerson(Person p)
        {
            if (p.WindowsUserName == null)
            {
                p.WindowsUserName = string.Empty;
            }
            if (p.Email == null)
            {
                p.Email = string.Empty;
            }
            PicContext.Current.DataContext.Persons.InsertOnSubmit(p);
            PicContext.Current.DataContext.SubmitChanges();
        }

        public bool ResetPassword(string email, string password)
        {
            MD5 md5				= MD5.Create();
			byte[] bPassword	= md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password));
			string encodedPassword = System.Text.ASCIIEncoding.ASCII.GetString(bPassword);

            // set up a connection and command to update password
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.up_UserManager_ResetPassword", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@password", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@success", SqlDbType.Bit);
			cmd.Parameters["@success"].Direction = ParameterDirection.Output;

			cmd.Parameters["@email"].Value = email;
            cmd.Parameters["@password"].Value = encodedPassword;

			// execute the command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (bool) cmd.Parameters["@success"].Value;

        }

		public bool ResetPassword(string email, string password, Guid resetKey)
		{
			// set up a connection and command to update password
			SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_ResetPassword", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@password", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@success", SqlDbType.Bit);
			cmd.Parameters["@success"].Direction = ParameterDirection.Output;

			cmd.Parameters["@email"].Value = email;
			cmd.Parameters["@guid"].Value = resetKey;
			cmd.Parameters["@password"].Value = password;

			// execute the command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (bool) cmd.Parameters["@success"].Value;
		}

		public PersonInfoCollection Find(string name)
		{
			PersonInfoCollection col	= new PersonInfoCollection();

			SqlConnection cn			= new SqlConnection(this.context.Config.ConnectionString);
			SqlCommand cmd				= new SqlCommand("dbo.sp_Person_Find", cn);
			SqlDataReader dr			= null;
			cmd.CommandType				= CommandType.StoredProcedure;

			cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150);
			cmd.Parameters["@name"].Value = name;

			try
			{
				cn.Open();
				dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
				while (dr.Read())
				{
					int id			= int.Parse(dr["PersonId"].ToString());
					string fullName	= dr["FullName"].ToString();
					string email	= dr["Email"].ToString();
					col.Add(new PersonInfo(id, fullName, email));
				}
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (dr != null)
				{
					dr.Close();
				}
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

			return col;
		}

        public static string GetEncryptedPassword(string textPassword)
        {
            // encrypt the password
            MD5 md5 = MD5.Create();
            byte[] bPassword = md5.ComputeHash(Encoding.ASCII.GetBytes(textPassword));

            string pwd = Encoding.ASCII.GetString(bPassword);
            return pwd;
        }

        public List<Person> GetRecentUsers()
        {
            var q = from p in PicContext.Current.DataContext.GetRecentlySelectedUsers()
                    select p;
            return q.ToList<Person>();
        }
	}
}
