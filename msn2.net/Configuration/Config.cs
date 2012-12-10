using System;
using System.Data;
using System.Configuration;
using System.Runtime.Remoting;
using System.Collections;
using System.Data.SqlClient;
using System.Web.Caching;
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

	#region CONFIG CLASS

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
		private Hashtable userList = new Hashtable();
		
		#endregion

		#region Login

		public void Login(string storageUrl)
		{
			ProjectFServices.DataService dataService = new ProjectFServices.DataService();
			dataService.Url = storageUrl;

            //DataSet ds = dataService.Login("all@msn2.net", Environment.MachineName);

            //// Save config values
            //this.signinId				= new Guid(ds.Tables[0].Rows[0]["SigninId"].ToString());
            //this.machineId				= new Guid(ds.Tables[0].Rows[0]["MachineId"].ToString());

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

		//private string xmlConnectionString = @"User ID=projectflogin;Password=asd*%p\@ex(!;Persist Security Info=False;Initial Catalog=projectf;Data Source=barbrady;Provider=SQLOLEDB";
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

	#endregion

	#region CONFIGDATA class

	/// <summary>
	/// Base class for data stored in config system
	/// </summary>
	public class ConfigData
	{

		#region Declares

		private Guid itemKey = Guid.Empty;
		private ShellActionList shellActionList = new ShellActionList();
		
		#endregion

		#region Properties

		public virtual int IconIndex
		{
			get { return 0; }
		}

		public Guid ItemKey
		{
			get { return itemKey; }
			set { itemKey = value; }
		}

		#endregion

		/// <summary>
		/// List of actions associated with class
		/// </summary>
		[System.Xml.Serialization.XmlIgnore]
		public ShellActionList ShellActionList
		{
			get
			{
				return shellActionList;
			}
		}

		protected void AddShellAction(ShellAction action)
		{
			shellActionList.Add(action);
		}

		public static void Add(object sender, ConfigDataAddEventArgs e)
		{
		}

	}

	public class ConfigDataAddEventArgs: EventArgs
	{
		#region Declares

		private System.Windows.Forms.IWin32Window owner;
		private Data parent;
		private bool cancel;

		#endregion

		#region Constructor

		public ConfigDataAddEventArgs(System.Windows.Forms.IWin32Window owner, Data parent)
		{
			this.owner		= owner;
			this.parent		= parent;
		}

		#endregion

		#region Properties

		public System.Windows.Forms.IWin32Window Owner
		{
			get 
			{ 
				return owner;
			}
		}

		public Data Parent
		{
			get
			{
				return parent;
			}
		}

		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		#endregion
	}

	public class ShellAction
	{
		private string name;
		private string description;
		private string help;
		private EventHandler eventHandler;

		public ShellAction(string name, string description, string help, EventHandler eventHandler)
		{
			this.name			= name;
			this.description	= description;
			this.help			= help;
			if (eventHandler != null)
			{
				this.eventHandler	+= eventHandler;
			}
		}

		#region Properties

		public string Name
		{
			get 
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string Help
		{
			get
			{
				return help;
			}
			set
			{
				help = value;
			}
		}

		public EventHandler EventHandler
		{
			get
			{
				return eventHandler;
			}
		}

		#endregion

	}

	public class ShellActionList: System.Collections.ReadOnlyCollectionBase
	{
		// TODO: Change to use hash table
		public ShellAction this[string name]
		{
			get
			{
				foreach (ShellAction action in InnerList)
				{
					if (action.Name == name)
					{
						return action;
					}
				}

				return null; 
			}
		}

		internal void Add(ShellAction action)
		{
			InnerList.Add(action);
		}
	}


	#endregion

	#region MessengerGroupData

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class MessengerGroupData: msn2.net.Configuration.ConfigData
	{
		public MessengerGroupData()
		{}
	}

	#endregion

	#region MessengerContactData

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class MessengerContactData: msn2.net.Configuration.ConfigData
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
