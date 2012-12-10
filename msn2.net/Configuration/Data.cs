using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using msn2.net.Configuration;
using System.Text;
using msn2.net.Common;
using msn2.net.Configuration.ProjectFServices;
using System.Threading;

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

	public class Data //: System.Windows.Forms.TreeNode
	{
		#region Declares

		private Guid			id;
		private string			name;
		private Guid			parentId;
		private msn2.net.Configuration.ConfigData		configData;
		private string			url;
		
		private Guid			configId;
		private Guid			userId;
		private Guid			groupId;
		private Guid			machineId;
		private Guid			policyId;
		private Type			dataType;
		private string			storageUrl;

		ProjectFServices.DataService dataService = null;

		#endregion
		#region Constructors

		// used for top level node
		internal Data(Guid configId, Guid userId, Guid machineId, Guid policyId, string storageUrl)
		{
			this.configId		= configId;
			this.userId			= userId;
			this.groupId		= groupId;
			this.machineId		= machineId;
			this.policyId		= policyId;
			this.storageUrl		= storageUrl;

			dataService = new ProjectFServices.DataService();
			dataService.Url = storageUrl;
			dataService.Credentials = new System.Net.NetworkCredential("msn2guest", "rainier", "sp");
		}

		internal Data(DataSetDataItem.DataItemRow row, Type type, string storageUrl)
		{
			this.id				= row.Id;
			this.name			= row.Name;
			this.parentId		= row.ParentId;
			this.userId			= row.UserId;
			this.storageUrl		= storageUrl;

			if (!row.IsItemUrlNull())
				this.url		= row.ItemUrl;

			this.Text			= this.name;

			//dataService = new DataService();

			if (type != null && !row.IsItemTypeNull() && row.ItemType.Length > 0)
			{
				this.DataType	= type;

				// Get the data too
				if (!row.IsItemDataNull() && row.ItemData.Length > 0)
				{
					this.configData = (msn2.net.Configuration.ConfigData) msn2.net.Common.Utilities.DeserializeObject(row.ItemData, this.dataType);
				}
			}

			dataService = new ProjectFServices.DataService();
			dataService.Url = this.storageUrl;
			dataService.Credentials = new System.Net.NetworkCredential("msn2guest", "rainier", "sp");
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

		public string Text
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

		public msn2.net.Configuration.ConfigData ConfigData
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
			Trace.WriteLine(String.Format("nodeId: {0}", this.id), "GetChildren");

			string[] typeList = new string[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				typeList[i] = types[i].ToString();
			}
			
			DataSetDataItem ds = 
				dataService.GetChildren(this.id, this.userId, typeList);
			
			DataCollection ar = new DataCollection();

			foreach (DataSetDataItem.DataItemRow row in ds.DataItem.Rows)
			{
				// Get the item type from the list of types passed in
				Type itemType = null;
				foreach (Type currentType in types)
				{
					if (currentType != null)
					{
						if (currentType.ToString() == row.ItemType)
						{
							itemType = currentType;
							break;
						}
					}
				}

				// Create the item with the right type
				Data item = new Data(row, itemType, storageUrl);

				ar.Add(item);
			}

			return ar;                                    
		}

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

			DataSetDataItem ds = new DataSetDataItem();
			da.Fill(ds, "DataItem");

			if (ds.DataItem.Rows.Count > 0)
			{
				Data node = new Data((DataSetDataItem.DataItemRow) ds.DataItem.Rows[0], data.GetType(), storageUrl);
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

			DataSetDataItem ds = new DataSetDataItem();
			da.Fill(ds, "DataItem");

			if (ds.DataItem.Rows.Count > 0)
			{
				Data node = new Data((DataSetDataItem.DataItemRow) ds.DataItem.Rows[0], null, storageUrl);
				return node;
			}
			else
			{
				return null;
			}
            
		}

		#endregion

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
		#region Get Calls

		#region Overloads

		public Data Get(string name)
		{
			return Get(name, null, null, null, ConfigTreeLocation.UserConfigTree);
		}

		public Data Get(string name, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, null, null, configTreeLocation);
		}

		public Data Get(string name, Type type)
		{
			return Get(name, null, null, type);
		}

		public Data Get(string name, Type type, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, null, type, configTreeLocation);
		}

		public Data Get(string name, msn2.net.Configuration.ConfigData data, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, null, data, null, configTreeLocation);
		}

		public Data Get(string name, msn2.net.Configuration.ConfigData data, Type type)
		{
			return Get(name, null, data, type);
		}

		public Data Get(string name, msn2.net.Configuration.ConfigData data, Type type, ConfigTreeLocation configTreeLocation)
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

		public Data Get(string name, string url, msn2.net.Configuration.ConfigData data)
		{
			return Get(name, url, data, data.GetType());
		}

		public Data Get(string name, string url, msn2.net.Configuration.ConfigData data, ConfigTreeLocation configTreeLocation)
		{
			return Get(name, url, data, data.GetType(), configTreeLocation);
		}
	

		public Data Get(string name, string url, msn2.net.Configuration.ConfigData data, Type type)
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
		public Data Get(string name, string url, msn2.net.Configuration.ConfigData data, Type type, ConfigTreeLocation configTreeLocation)
		{
			string itemType = null;
			if (type != null)
				itemType = type.ToString();

			// Check if we have a data object
			string serializedData = null;
			Guid itemKey = Guid.Empty;
			if (data != null)
			{
				serializedData = msn2.net.Common.Utilities.SerializeObject(data);
				itemKey = data.ItemKey;
			}

			// Load from service
			DataSetDataItem ds = dataService.Get(this.id, this.userId, name, url, serializedData, itemType, itemKey);

			// Check for results
			if (ds.DataItem.Rows.Count > 0)
			{
				Data node = new Data((DataSetDataItem.DataItemRow) ds.DataItem.Rows[0], type, storageUrl);
				node.configData	= data;
				node.dataType	= type;
				return node;
			}
			else
			{
				return null;
			}
		}

		#endregion
		#region Save

		/// <summary>
		/// Saves a node
		/// </summary>
		public void Save()
		{
			// Start a new thread to do actual save
			Thread thread = new Thread(new ThreadStart(SaveThreadStart));
			thread.Start();
		}

		private void SaveThreadStart()
		{
			string serializedData = null;
			if (configData != null)
			{
				serializedData = msn2.net.Common.Utilities.SerializeObject(this.configData);
			}

			dataService.Save(this.id, this.name, serializedData, this.configData.GetType().ToString());
		}

		#endregion
		#region Delete

		/// <summary>
		/// Delete an item
		/// </summary>
		/// <param name="itemId">Guid of item to delete</param>
		public void Delete()
		{
			// Start delete on a new thread (don't block this request)
			Thread thread = new Thread(new ThreadStart(DeleteThreadStart));
			thread.Start();
		}

		private void DeleteThreadStart()
		{
			dataService.Delete(this.id);
		}

		#endregion
		#region LookupTheirGroupId

		private Hashtable cachedGroupIds = new Hashtable();

		private Guid LookupTheirGroupId(MessengerContactData contactData)
		{
			Guid groupId = Guid.Empty;

			// Check hash table
			if (cachedGroupIds.Contains(contactData.ContactId))
			{
				return (Guid) cachedGroupIds[contactData.ContactId];
			}

			// Lookup parent for my contact ID
			string sql = "select ParentId from FavoritesCategory where ItemType = @itemType and ItemKey = @itemKey and UserId = @userId";

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd   = new SqlCommand(sql, cn);

			cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			cmd.Parameters.Add("@itemKey", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);

			cmd.Parameters["@itemType"].Value		= contactData.GetType().ToString();
			cmd.Parameters["@itemKey"].Value		= this.userId;
			cmd.Parameters["@userId"].Value			= contactData.ContactId;

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
			
			if (dr.Read())
			{
                groupId = new Guid(dr[0].ToString());
			}
			cn.Close();

			cachedGroupIds.Add(contactData.ContactId, groupId);
            
			return groupId;
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

		public Data this[string name]
		{
			get 
			{
				foreach (Data item in InnerList)
				{
					if (item.Name == name)
						return item;
				}
				return null;
			}
		}

		public bool Contains(string name)
		{
			foreach (Data item in InnerList)
			{
				if (item.Name == name)
					return true;
			}

			return false;
		}

	}

	#endregion

}
