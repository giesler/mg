using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using pics.Controls;

namespace pics
{
	/// <summary>
	/// Summary description for CategoryManager.
	/// </summary>
	public class CategoryManager : System.Web.Services.WebService
	{
		public CategoryManager()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		public Category GetCategory(int categoryId)
		{
			SqlConnection cn				= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd					= new SqlCommand("p_Category_Get", cn);
			cmd.CommandType					= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			SqlDataReader dr				= cmd.ExecuteReader(CommandBehavior.SingleRow);

			Category category				= null;
			if (dr.Read())
			{
				category					= new Category(dr);
			}
			return category;

		}

		[WebMethod]
		public CategoryCollection GetCateogies(int categoryId, int minPictures)
		{
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// get a dataset with categories
			SqlConnection cn		= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd			= new SqlCommand("dbo.p_CategoryManager_GetCategories", cn);
			cmd.CommandType			= CommandType.StoredProcedure;

			cmd.Parameters.Add("@personId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@minPictures", SqlDbType.Int);

            cmd.Parameters["@personId"].Value		= pi.PersonID;
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@minPictures"].Value	= minPictures;

			// Read data
			cn.Open();
			SqlDataReader dr		= cmd.ExecuteReader();
			CategoryCollection cc	= new CategoryCollection();
			while (dr.Read())
			{
				cc.Add(new Category(dr));
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
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			SqlConnection cn	= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("p_Category_PictureCount", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@personId",  SqlDbType.Int);
			cmd.Parameters.Add("@totalCount", SqlDbType.Int);
			cmd.Parameters["@totalCount"].Direction	= ParameterDirection.Output;
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);

			cmd.Parameters["@categoryId"].Value	= categoryId;
			cmd.Parameters["@personId"].Value	= pi.PersonID;
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
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			SqlConnection cn	= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd		= new SqlCommand("p_Category_SubCategoryCount", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@personId",  SqlDbType.Int);
			cmd.Parameters.Add("@totalCount", SqlDbType.Int);
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);
			cmd.Parameters["@totalCount"].Direction	= ParameterDirection.Output;

			cmd.Parameters["@categoryId"].Value	= categoryId;
			cmd.Parameters["@personId"].Value	= pi.PersonID;
			cmd.Parameters["@recursive"].Value	= recursive;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			return (int) cmd.Parameters["@totalCount"].Value;
		}

		public Picture RandomPicture(int categoryId, bool recursive)
		{
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			SqlConnection cn		= new SqlConnection(Config.ConnectionString);
			SqlCommand cmd			= new SqlCommand("p_CategoryManager_RandomCategoryPicture", cn);
			cmd.CommandType			= CommandType.StoredProcedure;

			cmd.Parameters.Add("@PersonId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);
			cmd.Parameters.Add("@maxWidth", SqlDbType.Int);
			cmd.Parameters.Add("@maxHeight", SqlDbType.Int);
			cmd.Parameters.Add("@recursive", SqlDbType.Bit);

			cmd.Parameters["@PersonId"].Value		= pi.PersonID;
			cmd.Parameters["@categoryId"].Value		= categoryId;
			cmd.Parameters["@maxWidth"].Value		= 125;
			cmd.Parameters["@maxHeight"].Value		= 125;
			cmd.Parameters["@recursive"].Value		= recursive;

			cn.Open();
			SqlDataReader dr		= cmd.ExecuteReader();
			Picture picture			= null;

			if (dr.Read())
			{
				picture				= new Picture();
				picture.Filename	= dr["Filename"].ToString();
				picture.Height		= (int) dr["Height"];
				picture.Width		= (int) dr["Width"];				
			}

			dr.Close();
			cn.Close();

			return picture;

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

		public Category()
		{
		}

		public Category(SqlDataReader dr)
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

