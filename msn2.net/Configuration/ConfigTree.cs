using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using msn2.net.Configuration;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace msn2.net.Configuration
{

	#region ConfigTreeLocation enum

	public enum ConfigTreeLocation
	{
		UserConfigTree,
		GroupConfigTree,
		MachineConfigTree,
		PolicyConfigTree,
		CustomConfigTree
	}
		
	#endregion

	#region Data class

	public class Data: System.Windows.Forms.TreeNode
	{

		#region Declares

		private Guid			id;
		private string			name;
		private Guid			parentId;
		private ConfigData		configData;
		private string			url;
		
		private Guid			configId;
		private Guid			userId;
		private Guid			groupId;
		private Guid			machineId;
		private Guid			policyId;
		private Type			dataType;

		private const int ITEMDATA_SIZE = 2048;

		#endregion

		#region Constructors

		// used for top level node
		internal Data(Guid configId, Guid userId, Guid machineId, Guid policyId)
		{
			this.configId		= configId;
			this.userId			= userId;
			this.groupId		= groupId;
			this.machineId		= machineId;
			this.policyId		= policyId;
		}

		internal Data(DataSetCategory.FavoritesCategoryRow row, Type type)
		{
			this.id				= row.CategoryId;
			this.name			= row.CategoryName;
			this.parentId		= row.ParentId;
			this.userId			= row.UserId;
			if (!row.IsItemUrlNull())
				this.url		= row.ItemUrl;

			this.Text			= this.name;

			if (type != null && !row.IsItemTypeNull() && row.ItemType.Length > 0)
			{
				this.DataType	= type;

				// Get the data too
				if (!row.IsItemDataNull() && row.ItemData.Length > 0)
				{
					this.configData = (ConfigData) DeserializeObject(row.ItemData, this.dataType);
				}
			}
		}

		#endregion

		#region Properties

		public Guid Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public Guid ParentId
		{
			get { return parentId; }
			set { parentId = value; }
		}

		public Guid UserId
		{
			get { return userId; }
			set { userId = value; }
		}

/*		public object Object
		{
			get { return obj; }
			set { obj = value; }
		}
*/
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		public Type DataType
		{
			get { return dataType; }
			set { dataType = value; }
		}

		public ConfigData ConfigData
		{
			get { return configData; }
			set { configData = value; }
		}

		#endregion

		#region GetChildren calls

		#region Overloads

		public DataCollection GetChildren()
		{
			Type type = null;
			return GetChildren(type);
		}

		public DataCollection GetChildren(Type type)
		{
			Type[] types	= new Type[1];
			types[0]		= type;
			return GetChildren(types);
		}

		#endregion

		/// <summary>
		/// Returns the children of the specified type at the specified node
		/// </summary>
		/// <param name="type">Type of children</param>
		/// <returns>Filtered children</returns>
		public DataCollection GetChildren(Type[] types)
		{
			Trace.Write(String.Format("nodeId: {0}", this.id), "GetChildren");

			// Select all root categories for this user
			string sql = "select * from FavoritesCategory "
				+ "where UserId = @userId and ParentId = @nodeId"; //and ConfigTreeLocation = @configTreeLocation";
			if (types != null && types[0] != null)
				sql	= sql + " and ItemType in (" + TypeArrayToString(types) + ")";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
//			da.SelectCommand.Parameters.Add("@configTreeLocation", SqlDbType.NVarChar, 50);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@userId"].Value				= userId;
			da.SelectCommand.Parameters["@nodeId"].Value				= this.id;
//			da.SelectCommand.Parameters["@configTreeLocation"].Value	= this.configTreeLocation.ToString();

			DataSetCategory dsCategories = new DataSetCategory();
			da.Fill(dsCategories, "FavoritesCategory");

			DataCollection ar = new DataCollection();

			foreach (DataSetCategory.FavoritesCategoryRow row in dsCategories.FavoritesCategory.Rows)
			{
				// Get the item type from the list of types passed in
				Type itemType = null;
				foreach (Type currentType in types)
				{
					if (currentType.ToString() == row.ItemType)
					{
						itemType = currentType;
						break;
					}
				}

				// Create the item with the right type
				Data item = new Data(row, itemType);

				ar.Add(item);
			}

			return ar;                                    
		}

		#region Utility functions

		private string TypeArrayToString(Type[] types)
		{
			if (types.Length == 0)
				return "";

			StringBuilder sb	= new StringBuilder();
			bool firstItem		= true;

			foreach (Type type in types)
			{
				if (type != null)
				{
					if (firstItem)
					{
						sb.Append("'" + type.ToString() + "'");
						firstItem = false;
					}
					else
					{
						sb.Append(", '" + type.ToString() + "'");
					}
				}
			}

			return sb.ToString();
		}

		#endregion

		#region GetItem

		public Data GetItem(string itemName, object data, ConfigTreeLocation configTreeLocation)
		{

			Trace.Write(String.Format("nodeId: {0}", this.id), "GetNode");

			// Select this node
			string sql = "select * from FavoritesCategory where ParentCategoryId = @parentId "
				+ " and ItemName = @itemName and ConfigTreeLocation = @configTreeLocation";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@configTreeLocation", SqlDbType.NVarChar, 50);
			da.SelectCommand.Parameters.Add("@itemName", SqlDbType.NVarChar, 255);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@parentId"].Value				= this.id;
			da.SelectCommand.Parameters["@configTreeLocation"].Value	= GetConfigTreeLocation(configTreeLocation);
			da.SelectCommand.Parameters["@itemName"].Value				= itemName;

			DataSetCategory ds = new DataSetCategory();
			da.Fill(ds, "FavoritesCategory");

			if (ds.FavoritesCategory.Rows.Count > 0)
			{
				Data node = new Data((DataSetCategory.FavoritesCategoryRow) ds.FavoritesCategory.Rows[0], data.GetType());
				return node;
			}
			else
			{
				return null;
			}
		}

		internal Data GetItem(Guid itemId)
		{
			Trace.Write(String.Format("nodeId: {0}", this.id), "GetItem");

			// Select this node
			string sql = "select * from FavoritesCategory where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@nodeId"].Value				= this.id;

			DataSetCategory ds = new DataSetCategory();
			da.Fill(ds, "FavoritesCategory");

			if (ds.FavoritesCategory.Rows.Count > 0)
			{
				Data node = new Data((DataSetCategory.FavoritesCategoryRow) ds.FavoritesCategory.Rows[0], null);
				return node;
			}
			else
			{
				return null;
			}
            
		}

		#endregion

		#endregion

		#region Serialize

		private string SerializeObject(object obj)
		{
			if (obj == null)
				return "";

			// Declares
			StringBuilder sb			= new StringBuilder();
			MemoryStream memStream		= new MemoryStream();
			XmlSerializer ser			= new XmlSerializer(obj.GetType());

			//System.Runtime.Serialization.Formatters.Soap.SoapFormatter ser =
			//	new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

			// Serialize into memory stream
			ser.Serialize(memStream, obj);

			// Copy into a string
			int position = 0;
			byte[] buffer = new byte[1024];
			memStream.Seek(0, SeekOrigin.Begin);
			while (position < memStream.Length)
			{
				// read a block
				int read = memStream.Read(buffer, position, 1024);
                
				// write to string
				for (int i = 0; i < read; i++)
					sb.Append( (char) buffer[i] );

				// update pointer
				position += read;
			}

			memStream.Close();
            
			return sb.ToString();
		}

		private object DeserializeObject(string xml, Type type)
		{

			// Declares
			XmlSerializer ser			= new XmlSerializer(type);
			byte[] buffer				= new byte[xml.Length];

			// Copy xml into byte array
			for (int i = 0; i < xml.Length; i++)
			{
				buffer[i] = (byte) xml[i];
			}

			// Deserialize the object
			MemoryStream memStream		= new MemoryStream(buffer);
			object obj					= ser.Deserialize(memStream);
			memStream.Close();

			return obj;
		}

		#endregion

		#region GetConfigTreeLocation

		private Guid GetConfigTreeLocation(ConfigTreeLocation requested)
		{

			if (requested == ConfigTreeLocation.UserConfigTree)
			{
				return userId;
			}

			if (requested == ConfigTreeLocation.GroupConfigTree)
			{
				return groupId;
			}

			if (requested == ConfigTreeLocation.MachineConfigTree)
			{
				return machineId;
			}

			if (requested == ConfigTreeLocation.PolicyConfigTree)
			{
				return policyId;
			}

			return userId;
		}

		#endregion

		#region Add Calls

		#region Overloads

		public Data Get(string name)
		{
			return Get(name, null, null);
		}

		public Data Get(string name, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, null, configTreeLocation);
		}

		public Data Get(string name, Type type)
		{
			return Get(name, null, null, type);
		}

		public Data Get(string name, Type type, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, null, type, configTreeLocation);
		}

		public Data Get(string name, object data, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, data, null, configTreeLocation);
		}

		public Data Get(string name, object data, Type type)
		{
			return Get(name, null, data, type);
		}

		public Data Get(string name, object data, Type type, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, data, type, configTreeLocation);
		}

		public Data Get(string name, string url)
		{
			return Get(name, url, null, null);
		}

		public Data Get(string name, string url, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, url, null, null, configTreeLocation);
		}

		public Data Get(string name, string url, Type type)
		{
			return Get(name, url, null, type);
		}

		public Data Get(string name, string url, Type type, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, url, null, type, configTreeLocation);
		}

		public Data Get(string name, string url, object data)
		{
			return Get(name, url, data, data.GetType());
		}

		public Data Get(string name, string url, object data, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, url, data, data.GetType(), configTreeLocation);
		}
	

		public Data Get(string name, string url, object data, Type type)
		{
			return Get(name, url, data, type, ConfigTreeLocation.UserConfigTree);
		}

		#endregion
		
		/// <summary>
		/// Adds a new child category
		/// </summary>
		/// <param name="name">New category name</param>
		/// <param name="parentId">Guid of the parent category</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>New cateogry Guid</returns>
		public Data Get(string name, string url, object data, Type type, ConfigTreeLocation configTreeLocation)
		{
			// Get a new cat id and guids for params
			Guid newCategoryId	= Guid.NewGuid();

			// insert new child cat
			string sql = "dbo.sp_Config_Get";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.CommandType		= CommandType.StoredProcedure;

			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			da.SelectCommand.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);
			da.SelectCommand.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			da.SelectCommand.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);
			da.SelectCommand.Parameters.Add("@configTreeLocation", SqlDbType.UniqueIdentifier);

			// Set command params
			da.SelectCommand.Parameters["@categoryId"].Value			= newCategoryId;
			da.SelectCommand.Parameters["@userId"].Value				= userId;
			da.SelectCommand.Parameters["@categoryName"].Value			= name;
			da.SelectCommand.Parameters["@parentId"].Value				= this.id;
			da.SelectCommand.Parameters["@configTreeLocation"].Value	= GetConfigTreeLocation(configTreeLocation);

			// Set url only if it has a value
			if (url != null)
			{
				da.SelectCommand.Parameters["@itemUrl"].Value	= url;
			}
			else
			{
				da.SelectCommand.Parameters["@itemUrl"].Value	= "";
			}

			// Lookup data only if it has a value - otherwise blank it
			if (data != null)
			{
				da.SelectCommand.Parameters["@itemData"].Value	= SerializeObject(data);
			}
			else
			{
				da.SelectCommand.Parameters["@itemData"].Value	= "";
			}

			// Save type only if it has a value - otherwise blank it
			if (type != null)
			{
				da.SelectCommand.Parameters["@itemType"].Value	= type.ToString();
			}
			else
			{
				da.SelectCommand.Parameters["@itemType"].Value	= "";
			}

			DataSetCategory ds = new DataSetCategory();
			da.Fill(ds, "FavoritesCategory");

			if (ds.FavoritesCategory.Rows.Count > 0)
			{
				Data node = new Data((DataSetCategory.FavoritesCategoryRow) ds.FavoritesCategory.Rows[0], type);
				return node;
			}

			return null;
		}

		#endregion

		#region Save

		/// <summary>
		/// Saves a node
		/// </summary>
		public void Save()
		{
			// insert new child cat
			string sql = "update FavoritesCategory set CategoryName = @categoryName ";
			if (this.configData != null)
			{
				sql		= sql + ", ItemData = @itemData, ItemType = @itemType ";
			}
			sql			= sql + "where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			if (this.configData != null)
			{
				cmd.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);
				cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			}

			// Set command params
			cmd.Parameters["@nodeId"].Value			= this.id;
			cmd.Parameters["@categoryName"].Value	= this.Text;
			if (this.configData != null)
			{
				cmd.Parameters["@itemData"].Value	= SerializeObject(this.configData);
				cmd.Parameters["@itemType"].Value	= this.configData.GetType().ToString();
			}

			// Run the command to update the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

		#region Delete

		/// <summary>
		/// Delete an item
		/// </summary>
		/// <param name="itemId">Guid of item to delete</param>
		public void Delete()
		{
			// update record
			string sql = "delete from FavoritesItem "
				+ "where CategoryId = @id";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@id"].Value	= id;

			// Run the command to update the rec
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

	}

	#endregion

	#region DataCollection class

	[Serializable()]
	public class DataCollection: System.Collections.ReadOnlyCollectionBase
	{
		public DataCollection()
		{
		}

		public void Add(Data item)
		{
			InnerList.Add(item);
		}

		public Data this[int index]
		{
			get { return (Data) InnerList[index]; }
		}

	}

	#endregion

	#region ConfigData

	public class ConfigData
	{
		public int IconIndex
		{
			get { return 0; }
		}
	}

	#endregion

}
