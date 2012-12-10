using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using msn2.net.Configuration;
using System.Text;

namespace msn2.net.ProjectF.Services
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	public class DataService : System.Web.Services.WebService
	{
		private const int ITEMDATA_SIZE = 2048;

		public DataService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region GetChildren

		[WebMethod]
		public DataSetCategory GetChildren(Guid nodeId, Guid userId, Type [] types)
		{
			// Select all items with this as the parent
			string sql = "select fc.* from FavoritesCategory fc inner join ItemParent ip on fc.CategoryId = ip.ItemId "
				+ "where ip.ParentId = @nodeId";
			
			if (types != null && types[0] != null)
				sql	= sql + " and ItemType in (" + TypeArrayToString(types) + ")";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da	= new SqlDataAdapter(sql, cn);
			//			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			//			da.SelectCommand.Parameters.Add("@configTreeLocation", SqlDbType.NVarChar, 50);

			// Set params to pass to SQL
			//			da.SelectCommand.Parameters["@userId"].Value				= userId;
			da.SelectCommand.Parameters["@nodeId"].Value				= nodeId;
			//			da.SelectCommand.Parameters["@configTreeLocation"].Value	= this.configTreeLocation.ToString();

			DataSetCategory dsCategories = new DataSetCategory();
			da.Fill(dsCategories, "FavoritesCategory");

			return dsCategories;
		}

		#endregion

		#region Get

		[WebMethod]
		public DataSetCategory Get(Guid nodeId, Guid userId, string name, string url, ConfigData data, Type type)
		{
			// Get a new cat id and guids for params
			Guid newCategoryId	= Guid.NewGuid();

			// insert new child cat
			string sql = "dbo.sp_Config_Get";

			SqlConnection cn				= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlDataAdapter da				= new SqlDataAdapter(sql, cn);
			da.SelectCommand.CommandType	= CommandType.StoredProcedure;

			// Set up sp params
			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			da.SelectCommand.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@itemUrl", SqlDbType.NVarChar, 500);
			da.SelectCommand.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			da.SelectCommand.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);
			//da.SelectCommand.Parameters.Add("@configTreeLocation", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters.Add("@itemAdded", SqlDbType.Bit);
			da.SelectCommand.Parameters.Add("@itemKey", SqlDbType.UniqueIdentifier);
			da.SelectCommand.Parameters["@itemAdded"].Direction			= ParameterDirection.Output;

			// Set command params values
			da.SelectCommand.Parameters["@categoryId"].Value			= newCategoryId;
			da.SelectCommand.Parameters["@userId"].Value				= userId;
			da.SelectCommand.Parameters["@categoryName"].Value			= name;
			da.SelectCommand.Parameters["@parentId"].Value				= nodeId;
			//da.SelectCommand.Parameters["@configTreeLocation"].Value	= GetConfigTreeLocation(configTreeLocation);
			da.SelectCommand.Parameters["@itemKey"].Value				= Guid.Empty;

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
				da.SelectCommand.Parameters["@itemData"].Value	= msn2.net.Configuration.Data.SerializeObject(data);
				if (data != null)
					da.SelectCommand.Parameters["@itemKey"].Value	= data.ItemKey;
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

//			if (ds.FavoritesCategory.Rows.Count > 0)
//			{
//				Data node = new Data((DataSetCategory.FavoritesCategoryRow) ds.FavoritesCategory.Rows[0], type);
//				
//				// Check if we need to update parents of other nodes
//				if (Convert.ToBoolean(da.SelectCommand.Parameters["@itemAdded"].Value))
//				{
//					Type[] types	= new Type[1];
//					types[0]		= typeof(msn2.net.Configuration.MessengerContactData);
//					DataCollection col = GetChildren(nodeId, userId, types);
//					foreach (Data item in col)
//					{
//						// We need to add a link for this contact
//						MessengerContactData contactData = (MessengerContactData) item.ConfigData;
//					
//						// Find out what group they are using for us
//						Guid theirGroupId = LookupTheirGroupId(contactData);
//
//						// Add a row to link these
//						sql = "insert ItemParent (ItemId, ParentId) values (@itemId, @parentId)";
//						SqlCommand cmd = new SqlCommand(sql, cn);
//						cmd.Parameters.Add("@itemId", SqlDbType.UniqueIdentifier);
//						cmd.Parameters.Add("@parentId", SqlDbType.UniqueIdentifier);
//						cmd.Parameters["@itemId"].Value = nodeId;
//						cmd.Parameters["@parentId"].Value	= theirGroupId;
//
//						cn.Open();
//						cmd.ExecuteNonQuery();
//						cn.Close();
//					}
//				}
				
//				return ds;
//			}

			return null;

		}

		#endregion

		#region Save

		[WebMethod]
		public void Save(Guid id, string name, ConfigData configData, Type type)
		{
			// insert new child cat
			string sql = "update FavoritesCategory set CategoryName = @categoryName ";
			if (configData != null)
			{
				sql		= sql + ", ItemData = @itemData, ItemType = @itemType ";
			}
			sql			= sql + "where CategoryId = @nodeId";

			SqlConnection cn	= new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd		= new SqlCommand(sql, cn);

			cmd.Parameters.Add("@nodeId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 250);
			if (configData != null)
			{
				cmd.Parameters.Add("@itemData", SqlDbType.NText, ITEMDATA_SIZE);
				cmd.Parameters.Add("@itemType", SqlDbType.NVarChar, 255);
			}

			// Set command params
			cmd.Parameters["@nodeId"].Value			= id;
			cmd.Parameters["@categoryName"].Value	= name;
			if (configData != null)
			{
				cmd.Parameters["@itemData"].Value	= msn2.net.Configuration.SerializeObject(configData);
				cmd.Parameters["@itemType"].Value	= type.ToString();
			}

			// Run the command to update the db row
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

		#endregion

		#region Delete

		[WebMethod]
		public void Delete(Guid id)
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

	}
}
