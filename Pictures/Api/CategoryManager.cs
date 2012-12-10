using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;

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
            int personId = PicContext.Current.CurrentUser.Id;

            HttpContext context = HttpContext.Current;
            Category category = null;
            string cacheKey = "Category." + categoryId.ToString() + ".Person." + personId.ToString();

            if (context != null)
            {
                object cacheObject = context.Cache[cacheKey];
                if (cacheObject != null)
                {
                    category = cacheObject as Category;
                    return category;
                }
            }            

			SqlConnection cn				= new SqlConnection(connectionString);
			SqlCommand cmd					= new SqlCommand("p_Category_Get", cn);
			cmd.CommandType					= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters["@categoryId"].Value	= categoryId;
            cmd.Parameters.Add("@personId", SqlDbType.Int);
            cmd.Parameters["@personId"].Value = personId;

			cn.Open();
			SqlDataReader dr				= cmd.ExecuteReader(CommandBehavior.SingleRow);

			if (dr.Read())
			{
				category					= new Category(dr, true);
			}

			dr.Close();
			cn.Close();

            if (context != null)
            {
                context.Cache.Add(cacheKey, category, null, DateTime.MaxValue, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.Normal, null);
            }

			return category;

		}

        public Category GetRootCategory()
        {
            return this.GetCategory(1);
        }

        public List<Category> GetCategories(int categoryId)
        {
            return this.GetCategories(categoryId, 0);
        }

        public List<Category> GetCategories(int categoryId, int minPictures)
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
			List<Category> cc	= new List<Category>();
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

		public List<Category> RecentCategorires()
		{
			SqlConnection cn  = new SqlConnection(connectionString);
			SqlCommand cmd    = new SqlCommand("dbo.sp_RecentCategories", cn);
			SqlDataReader dr = null;
			cmd.CommandType   = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);
			cmd.Parameters["@PersonID"].Value = PicContext.Current.CurrentUser.Id;

			List<Category> categories = new List<Category>();
            
			try
			{
				cn.Open();
				dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
				while (dr.Read())
				{
					categories.Add(new Category(dr, false));
				}
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

			return categories;
		}

		public string[] GetCategoryGroups(int categoryId)
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

			string[] groups = new string[ds.Tables[0].Rows.Count];
			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				groups[i] = ds.Tables[0].Rows[i][0].ToString();
			}

			return groups;
		}


	}


}

