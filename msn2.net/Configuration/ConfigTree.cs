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

	/// <summary>
	/// Summary description for TreeNode.
	/// </summary>
	public class ConfigTree ///: System.Windows.Forms.TreeView
	{
		private Guid userId = Guid.Empty;
		private ConfigTreeNode treeRoot = null;

		private const int ITEMDATA_SIZE = 2048;

		#region Constructor

		internal ConfigTree(Guid configId, Guid userId)
		{
			this.userId		= userId;
			this.treeRoot	= new ConfigTreeNode(configId, true);
		}

		#endregion

	}

	#region TreeNode class

	public class ConfigTreeNode: System.Windows.Forms.TreeNode
	{
		#region Declares

		private Guid	id;
		private string	name;
		private Guid	parentId;
		private Guid	userId;
		private object	obj;
		private string	url;

		#endregion

		#region Constructors

		// used for top level node
		internal ConfigTreeNode(Guid categoryId, bool skipme)
		{
			this.id		= categoryId;
			skipme		= !skipme;
		}

		public ConfigTreeNode(DataSetCategory.FavoritesCategoryRow row)
		{
			this.id				= row.CategoryId;
			this.name			= row.CategoryName;
			this.parentId		= row.ParentId;
			this.userId			= row.UserId;
			if (!row.IsItemUrlNull())
				this.url		= row.ItemUrl;

			this.Text			= this.categoryName;
		}

//		public ConfigTreeNode(Guid categoryId, string categoryName, Guid parentId, Guid userId)
//		{
//			this.categoryId		= categoryId;
//			this.categoryName	= categoryName;
//			this.parentId		= parentId;
//			this.userId			= userId;
//
//			this.Text			= this.categoryName;
//		}
//
//		public ConfigTreeNode(ConfigTreeNode parentNode, Guid categoryId, string categoryName, Guid userId)
//		{
//            this.categoryId		= categoryId;
//			this.categoryName	= categoryName;
//			this.parentId		= parentNode.parentId;
//			this.userId			= userId;
//		
//			this.Text			= this.categoryName;
//		}

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

		public object Object
		{
			get { return obj; }
			set { obj = value; }
		}

		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		#endregion

		#region Get calls

		/// <summary>
		/// Returns the children of the specified node
		/// </summary>
		/// <param name="nodeId">Parent node</param>
		/// <returns>All children</returns>
		public ConfigTreeNodeCollection GetChildren(ConfigTreeNode node)
		{
			return GetChildren(node, null);
		}

		/// <summary>
		/// Returns the children of the specified type at the specified node
		/// </summary>
		/// <param name="nodeId">Parent node</param>
		/// <param name="type">Type of children</param>
		/// <returns>Filtered children</returns>
		public ConfigTreeNodeCollection GetChildren(ConfigTreeNode node, Type type)
		{
			if (node != null)
			{
				Trace.Write(String.Format("nodeId: {0}", node.CategoryId), "GetChildren");
			}
			else 
			{
				Trace.Write(treeRoot.CategoryId, "GetChildren - Root level");
			}

			// Use the root if we were passed null
			if (node == null)
				node = treeRoot;

			// Select all root categories for this user
			string sql = "select * from FavoritesCategory "
				+ "where UserId = @userId and ParentId = @nodeId";
			if (type != null)
				sql	= sql + " and ItemType = @itemType";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			if (type != null)
				da.SelectCommand.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@userId"].Value	= userId;
			da.SelectCommand.Parameters["@nodeId"].Value	= node.CategoryId;
			if (type != null)
				da.SelectCommand.Parameters["@itemType"].Value	= type.ToString();

			DataSetCategory dsCategories = new DataSetCategory();
			da.Fill(dsCategories, "FavoritesCategory");

			ConfigTreeNodeCollection ar = new ConfigTreeNodeCollection();

			foreach (DataSetCategory.FavoritesCategoryRow row in dsCategories.FavoritesCategory.Rows)
			{
				ConfigTreeNode item = new ConfigTreeNode(row);
				
				// Check if we should deserialize any objects
				if (type != null)
				{
					if (!row.IsItemDataNull() && row.ItemData.Length > 0)
					{
						item.Object = DeserializeObject(row.ItemData, type);
					}
				}

				ar.Add(item);
			}

			return ar;                                    
		}

		internal ConfigTreeNode GetNode(Guid nodeId)
		{
			Trace.Write(String.Format("nodeId: {0}", nodeId), "GetNode");

			// Select this node
			string sql = "select * from FavoritesCategory where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@nodeId"].Value	= nodeId;

			DataSetCategory ds = new DataSetCategory();
			da.Fill(ds, "FavoritesCategory");

			if (ds.FavoritesCategory.Rows.Count > 0)
			{
				ConfigTreeNode node = new ConfigTreeNode((DataSetCategory.FavoritesCategoryRow) ds.FavoritesCategory.Rows[0]);
				return node;
			}
			else
			{
				return null;
			}
		}

		public object GetObjectAtNode(Guid nodeId, Type type)
		{
			Trace.Write(String.Format("nodeId: {0}", nodeId), "GetObjectAtNode");

			// Select all root categories for this user
			string sql = "select itemData, itemType from FavoritesCategory where CategoryId = @nodeId";
			object obj = null;

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);
			cmd.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);

			// Set params to pass to SQL
			cmd.Parameters["@nodeId"].Value	= nodeId;

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.Read())
			{
				if (!dr.IsDBNull(0))
				{
					obj = DeserializeObject(dr[0].ToString(), type);
				}
			}
			cn.Close();

			return obj;
		}

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

		#region Add Calls

		/// <summary>
		/// Adds a new root category
		/// </summary>
		/// <param name="categoryName">New category name</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>New category Guid</returns>
		//		public ConfigTreeNode AddRootNode(string name)
		//		{
		//			return AddChild(treeRoot, name, null, null);
		//		}
		//
		//		public ConfigTreeNode AddRootNode(string name, Type type)
		//		{
		//			return AddChild(treeRoot, name, null, null, type);
		//		}
		//
		//		public ConfigTreeNode AddRootNode(string name, object data)
		//		{
		//			return AddChild(treeRoot, name, null, data, data.GetType());
		//		}

		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name)
		{
			return AddChild(parent, name, null, null);
		}

		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name, Type type)
		{
			return AddChild(parent, name, null, null, type);
		}

		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name, string url)
		{
			return AddChild(parent, name, url, null, null);
		}

		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name, string url, Type type)
		{
			return AddChild(parent, name, url, null, type);
		}

		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name, string url, object data)
		{
			return AddChild(parent, name, url, data, data.GetType());
		}
			
		/// <summary>
		/// Adds a new child category
		/// </summary>
		/// <param name="name">New category name</param>
		/// <param name="parentId">Guid of the parent category</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>New cateogry Guid</returns>
		public ConfigTreeNode AddChild(ConfigTreeNode parent, string name, string url, object data, Type type)
		{
			// Get a new cat id and guids for params
			Guid newCategoryId	= Guid.NewGuid();

			// insert new child cat
			string sql = "insert into FavoritesCategory (CategoryId, UserId, CategoryName, ParentId, ItemUrl, ItemType, ItemData) "
				+ "values (@categoryId, @userId, @categoryName, @parentId, @itemUrl, @itemType, @itemData)";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			cmd.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= newCategoryId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@categoryName"].Value	= name;

			// if we have a parent id, use it, otherwise use the root
			if (parent != null)
			{
				cmd.Parameters["@parentId"].Value	= parent.CategoryId;
			}
			else
			{
				cmd.Parameters["@parentId"].Value	= treeRoot.CategoryId;
			}

			// Set url only if it has a value
			if (url != null)
			{
				cmd.Parameters["@itemUrl"].Value	= url;
			}
			else
			{
				cmd.Parameters["@itemUrl"].Value	= "";
			}

			// Lookup data only if it has a value - otherwise blank it
			if (data != null)
			{
				cmd.Parameters["@itemData"].Value	= SerializeObject(data);
			}
			else
			{
				cmd.Parameters["@itemData"].Value	= "";
			}

			// Save type only if it has a value - otherwise blank it
			if (type != null)
			{
				cmd.Parameters["@itemType"].Value	= type.ToString();
			}
			else
			{
				cmd.Parameters["@itemType"].Value	= "";
			}


			// Run the command to add the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return GetNode(newCategoryId);			
		}

		#endregion

		#region Update

		/// <summary>
		/// Updates a category
		/// </summary>
		/// <param name="categoryId">Guid of the category</param>
		/// <param name="categoryName">New name</param>
		/// <param name="userId">Guid for current user</param>
		public void UpdateNode(ConfigTreeNode node)
		{
			// insert new child cat
			string sql = "update FavoritesCategory set CategoryName = @categoryName ";
			if (node.Object != null)
			{
				sql		= sql + ", ItemData = @itemData, ItemType = @itemType ";
			}
			sql			= sql + "where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			if (node.Object != null)
			{
				cmd.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);
				cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			}

			// Set command params
			cmd.Parameters["@nodeId"].Value			= node.CategoryId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@categoryName"].Value	= node.Text;
			if (node.Object != null)
			{
				cmd.Parameters["@itemData"].Value	= SerializeObject(node.Object);
				cmd.Parameters["@itemType"].Value	= node.Object.GetType();
			}

			// Run the command to update the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

		#region Delete

		/// <summary>
		/// Deletes a category
		/// </summary>
		/// <param name="categoryId">Guid of the category</param>
		/// <param name="categoryName">New name</param>
		/// <param name="userId">Guid for current user</param>
		public void DeleteNode(ConfigTreeNode node)
		{
			// insert new child cat
			string sql = "delete from FavoritesCategory "
				+ "where CategoryId = @categoryId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= node.CategoryId;

			// Run the command to add the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

		#region AddItem

		/// <summary>
		/// Adds a new item
		/// </summary>
		/// <param name="userId">Guid for current user</param>
		/// <param name="categoryId">Guid of the category</param>
		/// <param name="itemName">Name of new item</param>
		/// <param name="itemUrl">Url for new item</param>
		/// <returns>New cateogry Guid</returns>
		public string AddItem(Guid categoryId, string itemName, string itemUrl)
		{
			// Get new guid
			Guid newItemId		= Guid.NewGuid();

			// insert new child cat
			string sql = "insert into FavoritesItem (CategoryId, UserId, ItemName, ItemUrl, ItemId) "
				+ "values (@categoryId, @userId, @itemName, @itemUrl, @itemId)";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@itemName"].Value		= itemName;
			cmd.Parameters["@itemUrl"].Value		= itemUrl;
			cmd.Parameters["@itemId"].Value			= newItemId;

			// Run the command to add the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return newItemId.ToString();			
		}

		#endregion

		#region UpdateItem

		/// <summary>
		/// Updates an item
		/// </summary>
		/// <param name="userId">Guid for current user</param>
		/// <param name="categoryName">New name</param>
		/// <param name="userId">Guid for current user</param>
		public void UpdateItem(ConfigTreeNode node)
		{
			// update record
			string sql = "update FavoritesCategory set CategoryName = @itemName, ItemUrl = @itemUrl, ItemData = @itemData "
				+ "where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@itemDAta", SqlDbType.NText, ITEMDATA_SIZE);

			// Set command params
			cmd.Parameters["@nodeId"].Value			= node.CategoryId;
			cmd.Parameters["@itemName"].Value		= node.Text;
			if (node.Url != null)
			{
				cmd.Parameters["@itemUrl"].Value	= node.Url;
			}
			else
			{
				cmd.Parameters["@itemUrl"].Value	= "";
			}

			if (node.Object != null)
			{
				cmd.Parameters["@itemData"].Value	= SerializeObject(node.Object);
			}
			else
			{
				cmd.Parameters["@itemData"].Value	= "";
			}

			// Run the command to update the rec
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}


		#endregion

		#region DeleteItem

		/// <summary>
		/// Delete an item
		/// </summary>
		/// <param name="itemId">Guid of item to delete</param>
		public void Delete()
		{
			// update record
			string sql = "delete from FavoritesItem "
				+ "where CategoryId = @categoryId and UserId = @userId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@itemId"].Value			= id;
			cmd.Parameters["@userId"].Value			= userId;

			// Run the command to update the rec
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion


	
	}

	#endregion

	#region ConfigTreeNodeCollection class

	[Serializable()]
	public class ConfigTreeNodeCollection: System.Collections.ReadOnlyCollectionBase
	{
		public ConfigTreeNodeCollection()
		{
		}

		public void Add(ConfigTreeNode item)
		{
			InnerList.Add(item);
		}

		public ConfigTreeNode this[int index]
		{
			get { return (ConfigTreeNode) InnerList[index]; }
		}

	}

	#endregion

	
}
