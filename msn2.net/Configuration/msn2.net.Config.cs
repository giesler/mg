using System;
using System.Data;
using System.Configuration;
using System.Runtime.Remoting;
using System.Collections;
using System.Data.SqlClient;
using System.Web.Caching;

namespace msn2.net.Configuration
{
	public class ConfigurationSettings
	{
		private static Config current;

		public static Config Current
		{
			get
			{
				if (current == null)
				{
//					lock (current)
					{
						if (current == null)
						{
							current = new Config();
						}
					}
				}
				return current;
			}
		}
	}

	/// <summary>
	/// Central configuration
	/// </summary>
	public class Config
	{
		private Guid userId;
		private Guid configId;
		private ItemIdDictionary itemIdDictionary;
		
		public void Login(Guid userId, Guid configId)
		{
			this.userId   = userId;
			this.configId = configId;

			itemIdDictionary = new ItemIdDictionary();
		}

		public Guid GetItemId(string itemName)
		{
            // Check for cache entry
			Guid itemId = itemIdDictionary.ItemId(itemName);

			// If we didn't get anything, look up in db
			if (itemId == Guid.Empty)
			{
				SqlConnection cn = new SqlConnection(ConnectionString);
				SqlCommand cmd   = new SqlCommand("s_Configuration_Item_Get", cn);
				cmd.CommandType  = CommandType.StoredProcedure;

				// Params for sp
				cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
				cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 50);
				cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
				cmd.Parameters["@itemId"].Direction = ParameterDirection.Output;

				// Set sp values
				cmd.Parameters["@userId"].Value			 = userId;
				cmd.Parameters["@itemName"].Value		 = itemName;

				// Run the sp
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();

				itemId = (Guid) cmd.Parameters["@itemId"].Value;
				itemIdDictionary.Add(itemName, itemId);
			}
			
			return itemId;

		}

		public ItemAttribute GetItemAttribute(string itemName, string attributeName)
		{
			return GetItemAttribute(itemName, attributeName, 0);
		}

		public ItemAttribute GetItemAttribute(string itemName, string attributeName, object defaultValue)
		{
			return GetItemAttribute(itemName, attributeName, 0, false);
		}

		public ItemAttribute GetItemAttribute(string itemName, string attributeName, object defaultValue, bool globalSetting)
		{
			SqlConnection cn = new SqlConnection(ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_Configuration_ItemAttribute_Get", cn);
			cmd.CommandType  = CommandType.StoredProcedure;

			Guid itemId = GetItemId(itemName);

			// Params for sp
			cmd.Parameters.Add("@configurationId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@attributeName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@defaultValue", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@attributeValue", SqlDbType.NVarChar, 50);
			cmd.Parameters["@attributeValue"].Direction = ParameterDirection.Output;

			// Set sp values
			if (globalSetting)
				cmd.Parameters["@configurationId"].Value = System.DBNull.Value;
			else
				cmd.Parameters["@configurationId"].Value = configId;
			cmd.Parameters["@itemId"].Value			 = itemId;
			cmd.Parameters["@attributeName"].Value	 = attributeName;
			cmd.Parameters["@defaultValue"].Value	 = defaultValue;

			// Run the sp
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return new ItemAttribute(cmd.Parameters["@AttributeValue"].Value);
		}

		public void SetItemAttribute(string itemName, string attributeName, object attributeValue)
		{
			SetItemAttribute(itemName, attributeName, attributeValue, false);
		}

		public void SetItemAttribute(string itemName, string attributeName, object attributeValue, bool globalSetting)
		{
			SqlConnection cn = new SqlConnection(ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_Configuration_ItemAttribute_Set", cn);
			cmd.CommandType  = CommandType.StoredProcedure;

			Guid itemId = GetItemId(itemName);

			// Params for sp
			cmd.Parameters.Add("@ConfigurationId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@ItemId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@AttributeValue", SqlDbType.NVarChar, 50);

			// Set sp values
			if (globalSetting)
				cmd.Parameters["@ConfigurationId"].Value = System.DBNull.Value;
			else
				cmd.Parameters["@ConfigurationId"].Value = configId;
			cmd.Parameters["@ItemId"].Value			 = itemId;
			cmd.Parameters["@AttributeName"].Value	 = attributeName;
			cmd.Parameters["@AttributeValue"].Value	 = attributeValue.ToString();

			// Run the sp
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
			
		}

		#region ConnectionString

		private string connectionString = "User ID=sa;Password=too;Persist Security Info=False;Initial Catalog=webdb;Data Source=kyle;";

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

		#region CurrentUser

		private string currentUser;

		/// <summary>
		/// Currently logged in user.
		/// </summary>
		/// <remarks>
		/// Broadcasts MSN2.net.LoginChangeEvent
		/// </remarks>
		public string CurrentUser
		{
			get
			{
				return currentUser;
			}
			set
			{
				currentUser = value;
				//save
			}
		}

		#endregion

		#region Logins

		private DataSet logins;

		/// <summary>
		/// Currently logged in user.
		/// </summary>
		/// <remarks>
		/// Broadcasts MSN2.net.LoginChangeEvent
		/// </remarks>
		public DataSet Logins
		{
			get
			{
				return logins;
			}
			set
			{
				logins = value;
				//save
			}
		}

        
		#endregion

		#region Forms

//		public static FormSettings FormS[string formName]
//		{
//			get 
//			{
//				
//			}
//		}

		#endregion

	}

	public class ItemAttribute
	{
		private object layoutAttribute;
	
		public ItemAttribute(object o)
		{
			this.layoutAttribute = o;
		}

		public int Integer
		{
			get 
			{
				return Convert.ToInt32(layoutAttribute);
			}
		}

		public string[] StringArray
		{
			get
			{
				return (layoutAttribute.ToString().Split(' '));
			}
		}
	}

	internal class ItemIdDictionary: System.Collections.DictionaryBase
	{
		public void Add(string itemName, Guid itemId)
		{
			this.Dictionary.Add(itemName, itemId);
		}

		public Guid ItemId(string itemName)
		{
			object o = this.Dictionary[itemName];

			if (o == null)
				return Guid.Empty;
			else
				return (Guid) o;
		}
	}

	public class FormSettingsCollection: System.Collections.CollectionBase
	{
		public FormSettings this[string name]
		{
			get
			{
				foreach (FormSettings f in InnerList)
				{
					if (f.Name == name)
						return f;
				}
				return null;
			}
		}
	}

	public class FormSettings
	{
		private string name;
		private int left;
		private int right;
		private int height;
		private int width;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int Left 
		{
			get { return left; }
			set { left = value; }
		}

		public int Right
		{
			get { return right; }
			set { right = value; }
		}

		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		public int Width
		{
			get { return width; }
			set { width = value; }
		}
	}
}
