using System;
using System.Data;
using System.Configuration;
using System.Runtime.Remoting;
using System.Collections;
using System.Data.SqlClient;
using System.Web.Caching;
using Microsoft.Data.SqlXml;
using System.Diagnostics;
using msn2.net.Common;

namespace msn2.net.Configuration
{
	#region Static Accessor

	public class ConfigurationSettings
	{
		private static Config current;

		public static Config Current
		{
			get
			{
				// Create the config on the first call
				if (current == null)
					current = new Config();

				return current;
			}
		}
	}

	#endregion

	/// <summary>
	/// Central configuration
	/// </summary>
	public class Config
	{
		
		#region Declares

		private string signinName	= "";
		private string machineName	= Environment.MachineName;
		private Guid signinId		= Guid.NewGuid();
		private Guid machineId		= Guid.NewGuid();
		private Guid policyId		= Guid.NewGuid();
		private Guid configId		= Guid.NewGuid();
		private Data data;
		private MessengerAPI.MessengerClass messenger = null;
		private Hashtable userList = new Hashtable();
		
		#endregion

		#region Login

		public void Login(MessengerAPI.MessengerClass messenger, string storageUrl)
		{
			this.messenger				= messenger;

			// Set up SP to retreive login IDs
			SqlXmlCommand cmd			= new SqlXmlCommand(xmlConnectionString);
			cmd.CommandText				= "EXEC dbo.s_Login ?, ?";
			cmd.RootTag					= "Login";

			// Create needed params
			SqlXmlParameter param;
			param						= cmd.CreateParameter();
			param.Value					= messenger.MySigninName;
			param						= cmd.CreateParameter();
			param.Value					= System.Environment.MachineName;

			// Create an adapter to fill a dataset
			SqlXmlAdapter xmlAdapter	= new SqlXmlAdapter(cmd);
			DataSet ds					= new DataSet();

			// Fill the dataset
			xmlAdapter.Fill(ds);

			// Save config values
			this.signinId				= new Guid(ds.Tables[0].Rows[0]["SigninId"].ToString());
			this.machineId				= new Guid(ds.Tables[0].Rows[0]["MachineId"].ToString());

			this.configId	= configId;
			this.policyId	= policyId;

			// load each tree
			data = new Data(configId, signinId, machineId, policyId, storageUrl);

		}

		#endregion

		#region Root Data node

		public Data Data
		{
			get { return data; }
		}

		#endregion

		#region ConnectionString

		private string xmlConnectionString = @"User ID=projectflogin;Password=asd*%p\@ex(!;Persist Security Info=False;Initial Catalog=projectf;Data Source=barbrady;Provider=SQLOLEDB";
		private string connectionString = @"User ID=projectflogin;Password=asd*%p\@ex(!;Persist Security Info=False;Initial Catalog=projectf;Data Source=barbrady";

		/// <summary>
		/// Get/set connectionString from config
		/// </summary>
		public string ConnectionString
		{
			get 
			{
				return connectionString ;
			}
			set
			{
				connectionString = value;
				//save
			}
		}

		#endregion

		#region SigninId

		/// <summary>
		/// Currently signed in user ID
		/// </summary>
		public Guid SigninId
		{
			get
			{
				return signinId;
			}
		}

		#endregion

		#region MySigninName

		public string MySigninName
		{
			get { return signinName; }
		}

		#endregion

		#region Messenger

		public MessengerAPI.MessengerClass Messenger
		{
			get { return messenger; }
		}

		#endregion

		#region GetSigninId

		public Guid GetSigninId(string signinName)
		{
			Guid signinId = Guid.Empty;

			// Check for user in hash table
			if (userList.Contains(signinName))
			{
				return (Guid) userList[signinName];
			}

			// Look up user in db
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("s_Lookup_SigninId", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@signInName", SqlDbType.NVarChar, 255);
			cmd.Parameters["@signInName"].Value = signinName;

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

			if (dr.Read())
			{
				signinId = new Guid(dr[0].ToString());
			}

			cn.Close();

			userList.Add(signinName, signinId);

			return signinId;
		}


		#endregion

	}

	#region MessengerGroupData

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class MessengerGroupData: msn2.net.Common.ConfigData
	{
		public MessengerGroupData()
		{}
	}

	#endregion

	#region MessengerContactData

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class MessengerContactData: msn2.net.Common.ConfigData
	{
		public MessengerContactData()
		{}

		public MessengerContactData(Guid contactId)
		{
			this.ItemKey	= contactId;
		}

		public Guid ContactId
		{
			get { return this.ItemKey; }
			set { this.ItemKey = value; }
		}

		public override int IconIndex
		{
			get { return 3; }
		}
	}

	#endregion

}
