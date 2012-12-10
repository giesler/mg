using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace msn2.net.Pictures
{
	/// <summary>
	/// Summary description for CategoryManager.
	/// </summary>
	public class CategoryManager
	{
		private string connectionString;

		public CategoryManager(string connectionString)
		{
			this.connectionString	= connectionString;
		}

		public Category GetCategory(int categoryId)
		{
			SqlConnection cn				= new SqlConnection(connectionString);
			SqlCommand cmd					= new SqlCommand("p_Category_Get", cn);
			cmd.CommandType					= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			SqlDataReader dr				= cmd.ExecuteReader(CommandBehavior.SingleRow);

			Category category				= null;
			if (dr.Read())
			{
				category					= new Category(dr, true);
			}

			dr.Close();
			cn.Close();

			return category;

		}

		public DataSetCategory GetCategoryDataSet(int categoryId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlDataAdapter da	= new SqlDataAdapter("dbo.p_Category_Get", cn);
			da.SelectCommand.CommandType	= CommandType.StoredProcedure;
			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.Int);
			da.SelectCommand.Parameters["@categoryId"].Value = categoryId;

			DataSetCategory ds = new DataSetCategory();
			da.Fill(ds, "Category");
		    
			return ds;
		}

		public CategoryCollection GetCategories(int categoryId, int minPictures)
		{
			// get a dataset with categories
			SqlConnection cn		= new SqlConnection(connectionString);
			SqlCommand cmd			= new SqlCommand("dbo.p_CategoryManager_GetCategories", cn);
			cmd.CommandType			= CommandType.StoredProcedure;

			cmd.Parameters.Add("@personId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@minPictures", SqlDbType.Int);

			cmd.Parameters["@personId"].Value		= PicContext.Current.CurrentUser.Id;
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@minPictures"].Value	= minPictures;

			// Read data
			cn.Open();
			SqlDataReader dr		= cmd.ExecuteReader();
			CategoryCollection cc	= new CategoryCollection();
			while (dr.Read())
			{
				cc.Add(new Category(dr, false));
			}
			dr.Close();
			cn.Close();

			return cc;
		}

		public int PictureCount(int categoryId)
		{
			return PictureCount(categoryId, false);
		}

		public int PictureCount(int categoryId, bool recursive)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("p_Category_PictureCount", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@personId",  SqlDbType.Int);
			cmd.Parameters.Add("@totalCount", SqlDbType.Int);
			cmd.Parameters["@totalCount"].Direction	= ParameterDirection.Output;
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);

			cmd.Parameters["@categoryId"].Value	= categoryId;
			cmd.Parameters["@personId"].Value	= PicContext.Current.CurrentUser.Id;
			cmd.Parameters["@recursive"].Value	= recursive;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (int) cmd.Parameters["@totalCount"].Value;
		}

		public int CategoryCount(int categoryId)
		{
			return CategoryCount(categoryId, false);
		}

		public int CategoryCount(int categoryId, bool recursive)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("p_Category_SubCategoryCount", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@personId",  SqlDbType.Int);
			cmd.Parameters.Add("@totalCount", SqlDbType.Int);
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);
			cmd.Parameters["@totalCount"].Direction	= ParameterDirection.Output;

			cmd.Parameters["@categoryId"].Value	= categoryId;
			cmd.Parameters["@personId"].Value	= PicContext.Current.CurrentUser.Id;
			cmd.Parameters["@recursive"].Value	= recursive;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (int) cmd.Parameters["@totalCount"].Value;
		}

		public PictureData RandomPicture(int categoryId, bool recursive)
		{
			SqlConnection cn		= new SqlConnection(connectionString);
			SqlCommand cmd			= new SqlCommand("p_CategoryManager_RandomCategoryPicture", cn);
			cmd.CommandType			= CommandType.StoredProcedure;

			cmd.Parameters.Add("@PersonId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@maxWidth", SqlDbType.Int);
			cmd.Parameters.Add("@maxHeight", SqlDbType.Int);
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);

			cmd.Parameters["@PersonId"].Value		= PicContext.Current.CurrentUser.Id;
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@maxWidth"].Value		= 125;
			cmd.Parameters["@maxHeight"].Value		= 125;
			cmd.Parameters["@recursive"].Value		= recursive;

			cn.Open();
			SqlDataReader dr		= cmd.ExecuteReader();
			PictureData picture			= null;

			if (dr.Read())
			{
				picture	= new PictureData(dr);
			}

			dr.Close();
			cn.Close();

			return picture;

		}

		public void SetCategoryPictureId(int categoryId, int pictureId)
		{
			SqlConnection cn  = new SqlConnection(connectionString);
			SqlCommand cmd    = new SqlCommand("dbo.sp_CategoryManager_SetCategoryPictureId", cn);
			cmd.CommandType   = CommandType.StoredProcedure;
			cmd.Parameters.Add("@CategoryId", SqlDbType.Int);
			cmd.Parameters.Add("@pictureId", SqlDbType.Int);

			cmd.Parameters["@categoryId"].Value = categoryId;
			cmd.Parameters["@pictureId"].Value	= pictureId;
            
			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

		}

		public void PublishCategory(int categoryId)
		{
			SqlConnection cn  = new SqlConnection(connectionString);
			SqlCommand cmd    = new SqlCommand("dbo.sp_CategoryManager_PublishCategory", cn);
			cmd.CommandType   = CommandType.StoredProcedure;
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);

			cmd.Parameters["@categoryId"].Value = categoryId;
            
			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

		}

		public DataSet RecentCategorires()
		{
			SqlConnection cn  = new SqlConnection(connectionString);
			SqlCommand cmd    = new SqlCommand("dbo.sp_RecentCategories", cn);
			cmd.CommandType   = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PersonID", PicContext.Current.CurrentUser.Id);
            
            DataSet ds	= new DataSet();

			try
			{
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(ds);
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

			return ds;
		}

		public DataSet GetCategoryGroups(int categoryId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlDataAdapter da	= new SqlDataAdapter("sp_CategoryManager_GetGruops", cn);
			DataSet ds			= new DataSet();
			da.SelectCommand.CommandType = CommandType.StoredProcedure;
			da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.Int);
			da.SelectCommand.Parameters["@categoryId"].Value	= categoryId;

			try
			{
				cn.Open();
				da.Fill(ds);
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

			return ds;
		}


	}

	[Serializable]
	public class Category
	{
		public int CategoryId;
		public string CategoryName;
		public int CategoryParentId;
		protected string description;
		protected int parentCategoryId;
		public int PictureId;
		public DateTime FromDate;
		public DateTime ToDate;

		public Category()
		{
		}

		public Category(SqlDataReader dr, bool includeDates)
		{
			CategoryId			= (int) dr["CategoryId"];
			CategoryName		= dr["CategoryName"].ToString();
			CategoryParentId	= (int) dr["CategoryParentId"];
			PictureId			= (int) dr["PictureId"];
			if (dr["CategoryDescription"] != null)
			{
				Description	= dr["CategoryDescription"].ToString();
			}
			parentCategoryId	= (int) dr["CategoryParentId"];
			
			if (includeDates)
			{
				if (dr["FromDate"] != System.DBNull.Value)
				{
					FromDate	= Convert.ToDateTime(dr["FromDate"]);
				}
				if (dr["ToDate"] != System.DBNull.Value)
				{
					ToDate		= Convert.ToDateTime(dr["ToDate"]);
				}
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
		
		public string Name
		{
			get 
			{
				return CategoryName;
			}
			set
			{
				CategoryName = value;
			}
		}

		public int ParentCategoryId
		{
			get
			{
				return parentCategoryId;
			}
			set
			{
				parentCategoryId = value;
			}
		}

		public override string ToString()
		{
			return CategoryName;
		}
	}

	public class CategoryCollection: ReadOnlyCollectionBase
	{
		public void Add(Category category)
		{
			InnerList.Add(category);
		}

		public Category this[string name]
		{
			get
			{
				foreach (Category category in InnerList)
				{
					if (category.Name == name)
						return category;
				}
				return null;
			}
		}

		public Category this[int index]
		{
			get
			{
				return (Category) InnerList[index];
			}
		}
	
	
	}

}

