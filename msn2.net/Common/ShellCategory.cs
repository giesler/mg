using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace msn2.net.Common
{

	/// <summary>
	/// Summary description for TreeNode.
	/// </summary>
	public class Tree: System.Windows.Forms.TreeView
	{
		private Guid userId = Guid.Empty;
		private Guid treeId = Guid.Empty;

		#region Constructor

		public Tree(Guid treeId, Guid userId)
		{
			this.treeId	= treeId;
			this.userId	= userId;
		}

		#endregion

		#region Get calls

		/// <summary>
		/// Returns the root categories
		/// </summary>
		/// <param name="userId">Guid for current user</param>
		/// <returns>DataSet containing child categories</returns>
		public CategoryTreeNodeCollection GetRootCategories()
		{
			return GetChildCategories(treeId, "msn2.net.Common.TreeNode");
		}

		/// <summary>
		/// Returns the contained categories
		/// </summary>
		/// <param name="categoryId">Guid of the category</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>DataSet containing child categories</returns>
		public CategoryTreeNodeCollection GetChildCategories(Guid categoryId, string itemType)
		{
			Trace.Write(String.Format("parentId: {0}", categoryId), "GetChildCategories");

			// Select all root categories for this user
			string sql = "select * from FavoritesCategory "
				+ "where UserId = @userId and ParentId = @parentId and ItemType = @itemType";

			SqlConnection cn	= new SqlConnection(Config.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);

			// Set params to pass to SQL
			da.SelectCommand.Parameters["@userId"].Value	= userId;
			da.SelectCommand.Parameters["@parentId"].Value	= categoryId;
			da.SelectCommand.Parameters["@itemType"].Value	= itemType;

			DataSetCategory dsCategories = new DataSetCategory();
			da.Fill(dsCategories, "FavoritesCategory");

			CategoryTreeNodeCollection ar = new CategoryTreeNodeCollection();

			foreach (DataSetCategory.FavoritesCategoryRow row in dsCategories.FavoritesCategory.Rows)
			{

				//TreeNode item = new TreeNode(row);
				ar.Add(new CategoryTreeNode(row));
			}

			return ar;                                    
		}

		#endregion

		#region Add Calls

		/// <summary>
		/// Adds a new root category
		/// </summary>
		/// <param name="categoryName">New category name</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>New category Guid</returns>
		public Guid AddRootCategory(string categoryName)
		{
			return AddChildCategory(categoryName, treeId, "msn2.net.Category");
		}

		/// <summary>
		/// Adds a new child category
		/// </summary>
		/// <param name="categoryName">New category name</param>
		/// <param name="parentId">Guid of the parent category</param>
		/// <param name="userId">Guid for current user</param>
		/// <returns>New cateogry Guid</returns>
		public Guid AddChildCategory(string categoryName, Guid parentId, string itemType)
		{
			// Get a new cat id and guids for params
			Guid newCategoryId	= Guid.NewGuid();

			// insert new child cat
			string sql = "insert into FavoritesCategory (CategoryId, UserId, CategoryName, ParentId, ItemType) "
				+ "values (@categoryId, @userId, @categoryName, @parentId, @itemType)";

			SqlConnection cn	= new SqlConnection(Config.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= newCategoryId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@categoryName"].Value	= categoryName;
			cmd.Parameters["@parentId"].Value		= parentId;
			cmd.Parameters["@itemType"].Value		= itemType;

			// Run the command to add the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return newCategoryId;			
		}

		#endregion

		#region Update

		/// <summary>
		/// Updates a category
		/// </summary>
		/// <param name="categoryId">Guid of the category</param>
		/// <param name="categoryName">New name</param>
		/// <param name="userId">Guid for current user</param>
		public void UpdateCategory(Guid categoryId, string categoryName)
		{
			// insert new child cat
			string sql = "update FavoritesCategory set CategoryName = @categoryName "
				+ "where CategoryId = @categoryId and UserId = @userId";

            SqlConnection cn = new SqlConnection(Config.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@categoryName"].Value	= categoryName;

			// Run the command to add the db row
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
		public void DeleteCategory(Guid categoryId)
		{
			// insert new child cat
			string sql = "delete from FavoritesCategory "
				+ "where CategoryId = @categoryId and UserId = @userId";

            SqlConnection cn = new SqlConnection(Config.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@userId"].Value			= userId;

			// Run the command to add the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

//		#region GetItem
//
//		/// <summary>
//		/// Returns items for the current user and category
//		/// </summary>
//		/// <param name="categoryId">Guid of current category</param>
//		/// <param name="userId">Guid of current user</param>
//		public DataSet GetItems(Guid categoryId)
//		{
//
//			// Select all root categories for this user
//			string sql = "select * from FavoritesItems where UserId = @userId and CategoryId = @categoryId";
//
//			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
//			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
//			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
//			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
//
//			// Set params to pass to SQL
//			da.SelectCommand.Parameters["@userId"].Value		= userId;
//			da.SelectCommand.Parameters["@categoryId"].Value	= categoryId;
//
//			DataSet items = new DataSetItem();
//			da.Fill(items);
//
//			return items;                                    
//
//		}
//
//		#endregion

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

            SqlConnection cn = new SqlConnection(Config.Current.ConnectionString);
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
		public void UpdateItem(Guid itemId, string itemName, string itemUrl)
		{
			// update record
			string sql = "update FavoritesItem sCategory set ItemName = @itemName and ItemUrl = @itemUrl "
				+ "where CategoryId = @categoryId and UserId = @userId";

            SqlConnection cn = new SqlConnection(Config.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@itemName", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);

			// Set command params
			cmd.Parameters["@itemId"].Value			= itemId;
			cmd.Parameters["@userId"].Value			= userId;
			cmd.Parameters["@itemName"].Value		= itemName;
			cmd.Parameters["@itemUrl"].Value		= itemUrl;

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
		public void DeleteItem(Guid itemId)
		{
			// update record
			string sql = "delete from FavoritesItem "
				+ "where CategoryId = @categoryId and UserId = @userId";

            SqlConnection cn = new SqlConnection(Config.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);

			// Set command params
			cmd.Parameters["@itemId"].Value			= itemId;
			cmd.Parameters["@userId"].Value			= userId;

			// Run the command to update the rec
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

	}

	#region TreeNode class

	[Serializable()]
	public class CategoryTreeNode: System.Windows.Forms.TreeNode
	{
		#region Declares

		private Guid	categoryId;
		private string	categoryName;
		private Guid	parentId;
		private Guid	userId;

		#endregion

		#region Constructors

		public CategoryTreeNode()
		{
		}

		public CategoryTreeNode(DataSetCategory.FavoritesCategoryRow row)
		{
			this.categoryId		= row.CategoryId;
			this.categoryName	= row.CategoryName;
			this.parentId		= row.ParentId;
			this.userId			= row.UserId;

			this.Text			= this.categoryName;
		}

		public CategoryTreeNode(Guid categoryId, string categoryName, Guid parentId, Guid userId)
		{
			this.categoryId		= categoryId;
			this.categoryName	= categoryName;
			this.parentId		= parentId;
			this.userId			= userId;

			this.Text			= this.categoryName;
		}

		public CategoryTreeNode(CategoryTreeNode parentNode, Guid categoryId, string categoryName, Guid userId)
		{
            this.categoryId		= categoryId;
			this.categoryName	= categoryName;
			this.parentId		= parentNode.parentId;
			this.userId			= userId;
		
			this.Text			= this.categoryName;
		}

		#endregion

		#region Properties

		public Guid CategoryId
		{
			get { return categoryId; }
			set { categoryId = value; }
		}

		public string CategoryName
		{
			get { return categoryName; }
			set { categoryName = value; }
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

		#endregion
	}

	#endregion

	#region CategoryTreeNodeCollection class

	[Serializable()]
	public class CategoryTreeNodeCollection: System.Collections.ReadOnlyCollectionBase
	{
		public CategoryTreeNodeCollection()
		{
		}

		public void Add(CategoryTreeNode item)
		{
			InnerList.Add(item);
		}

		public CategoryTreeNode this[int index]
		{
			get { return (CategoryTreeNode) InnerList[index]; }
		}

	}

	#endregion

}
