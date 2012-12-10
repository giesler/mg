using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace msn2.net.Pictures
{

    public enum PictureSortField
    {
        DatePictureTaken = 0,
        DatePictureAdded = 1,
        DatePictureUpdated = 2
    }

    public enum PictureSortOrder
    {
        SortAscending = 0,
        SortDescending = 1
    }

    /// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class PictureManager
	{
		private string connectionString;

		public PictureManager(string connectionString)
		{
			this.connectionString	= connectionString;
		}

		public void AddToCategory(int pictureId, int categoryId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("p_Picture_AddToCategory", cn);
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);

			cmd.Parameters["@pictureId"].Value	= pictureId;
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

		}

		public void RemoveFromCategory(int pictureId, int categoryId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("p_Picture_RemoveFromCategory", cn); 
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int);

			cmd.Parameters["@pictureId"].Value	= pictureId;
			cmd.Parameters["@categoryId"].Value	= categoryId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

		}

        public decimal RatePicture(int pictureId, int rating)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_SetRating", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@personId", SqlDbType.Int);
            cmd.Parameters.Add("@rating", SqlDbType.Int);
            cmd.Parameters.Add("@averageRating", SqlDbType.Decimal);
            cmd.Parameters["@averageRating"].Direction = ParameterDirection.Output;

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@personId"].Value = PicContext.Current.CurrentUser.Id;
            cmd.Parameters["@rating"].Value = rating;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

            decimal averageRating = (decimal) cmd.Parameters["@averageRating"].Value;
            return averageRating;
        }

		public void AddToSecurityGroup(int pictureId, int groupId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd		= new SqlCommand("p_Picture_AddToSecurityGroup", cn); 
			cmd.CommandType		= CommandType.StoredProcedure;

			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters.Add("@groupId", SqlDbType.Int);

			cmd.Parameters["@pictureId"].Value	= pictureId;
			cmd.Parameters["@groupId"].Value	= groupId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

        public void RemoveFromSecurityGroup(int pictureId, int groupId)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_RemoveFromSecurityGroup", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@groupId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@groupId"].Value = groupId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public void RotateImage(int pictureId, RotateFlipType rft)
        {
            PictureData data = GetPicture(pictureId);

            // Load iamge
            string appPath = PicContext.Current.Config.PictureDirectory;
            if (!appPath.Equals(@"\"))
                appPath = appPath + @"\";

            string file = appPath + data.Filename;

            using (Image image = Image.FromFile(file))
            {
                image.RotateFlip(rft);
                image.Save(file);
            }

        }

        public PictureDataSet GetPicture(int pictureId, int maxWidth, int maxHeight)
		{
			// Set up SP to retreive picture
			SqlConnection cn		= new SqlConnection(connectionString);
			SqlCommand cmdPic		= new SqlCommand("p_GetPicture", cn);
			cmdPic.CommandType		= CommandType.StoredProcedure;
			SqlDataAdapter daPic	= new SqlDataAdapter(cmdPic);

			// set up params on the SP
			cmdPic.Parameters.Add("@PictureID", SqlDbType.Int);
			cmdPic.Parameters.Add("@StartRecord", SqlDbType.Int);
			cmdPic.Parameters.Add("@ReturnCount", SqlDbType.Int);
			cmdPic.Parameters.Add("@MaxHeight", SqlDbType.Int);
			cmdPic.Parameters.Add("@MaxWidth", SqlDbType.Int);
			cmdPic.Parameters.Add("@PersonID", SqlDbType.Int);
			cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

			cmdPic.Parameters["@StartRecord"].Value = 0;
			cmdPic.Parameters["@ReturnCount"].Value = 1;
			cmdPic.Parameters["@PictureId"].Value = pictureId;
			cmdPic.Parameters["@MaxHeight"].Value = maxHeight;
			cmdPic.Parameters["@MaxWidth"].Value = maxWidth;
			cmdPic.Parameters["@PersonID"].Value = PicContext.Current.CurrentUser.Id;
			
			// run the SP, set datasource to the picture list
			cn.Open();
            PictureDataSet ds = new PictureDataSet();
            daPic.Fill(ds, "Picture");
            cn.Close();

            return ds;
		}

        public Image GetPictureImage(PictureData picture, int maxWidth, int maxHeight)
        {
            Image image = PicContext.Current.PictureCache.GetImage(picture, maxWidth, maxHeight);
            return image;
        }

        public void Save(PictureData pictureData)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("up_PictureManager_Save", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@title", SqlDbType.NVarChar, 255);
            cmd.Parameters.Add("@description", SqlDbType.NVarChar, 2000);
            cmd.Parameters.Add("@dateTaken", SqlDbType.DateTime);

            cmd.Parameters["@id"].Value = pictureData.Id;
            cmd.Parameters["@title"].Value = pictureData.Title;
            cmd.Parameters["@description"].Value = pictureData.Description;
            cmd.Parameters["@dateTaken"].Value = pictureData.DateTaken;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public PictureData GetPicture(int pictureId)
		{
			// init connection and command
			SqlConnection cn		= new SqlConnection(connectionString);

			// Set up SP to retreive pictures
			SqlCommand cmdPic		= new SqlCommand("dbo.up_PictureManager_GetPicture", cn);
			cmdPic.CommandType		= CommandType.StoredProcedure;
			SqlDataAdapter daPic	= new SqlDataAdapter(cmdPic);

			// Set up params for SP
			cmdPic.Parameters.Add("@pictureId", SqlDbType.Int);
			cmdPic.Parameters.Add("@personId", SqlDbType.Int);

			cmdPic.Parameters["@pictureId"].Value = pictureId;
			cmdPic.Parameters["@personId"].Value = PicContext.Current.CurrentUser.Id;

			// run the SP, set datasource to the picture list
            cn.Open();
            SqlDataReader dr = cmdPic.ExecuteReader(CommandBehavior.SingleRow);
            PictureData data = null;
            if (dr.Read())
            {
                data = new PictureData(dr);
            }
            dr.Close();
            cn.Close();

            return data;
        }

        public PictureData GetRandomPicture()
        {
            // init connection and command
            SqlConnection cn = new SqlConnection(connectionString);

            // Set up SP to retreive pictures
            SqlCommand cmdPic = new SqlCommand("dbo.p_RandomPicture", cn);
            cmdPic.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);

            // Set up params for SP
            cmdPic.Parameters.Add("@personId", SqlDbType.Int);
			cmdPic.Parameters["@personId"].Value = PicContext.Current.CurrentUser.Id;

            // run the SP, set datasource to the picture list
            cn.Open();
            SqlDataReader dr = cmdPic.ExecuteReader(CommandBehavior.SingleRow);
            PictureData data = null;
            if (dr.Read())
            {
                data = new PictureData(dr);
            }
            dr.Close();
            cn.Close();

            return data;
        }

        public PictureDataSet GetPicturesByCategory(int categoryId)
		{
			// init connection and command
			SqlConnection cn		= new SqlConnection(connectionString);

			// Set up SP to retreive pictures
			SqlCommand cmdPic		= new SqlCommand("up_PictureManager_GetPicturesByCategory", cn);
			cmdPic.CommandType		= CommandType.StoredProcedure;
			SqlDataAdapter daPic	= new SqlDataAdapter(cmdPic);

			// Set up params for SP
			cmdPic.Parameters.Add("@categoryId", SqlDbType.Int);
			cmdPic.Parameters.Add("@personId", SqlDbType.Int);

			cmdPic.Parameters["@categoryId"].Value = categoryId;
			cmdPic.Parameters["@personId"].Value = PicContext.Current.CurrentUser.Id;

			// run the SP, set datasource to the picture list
			PictureDataSet ds		= new PictureDataSet();
			daPic.Fill(ds, "Picture");

			return ds;
		}

        public PictureCollection GetPictures(string sqlWhereClause)
        {
            PictureCollection pictures = new PictureCollection();

            string sql = "select 1 as RecNumber, p.PictureID, p.PictureDate, p.Title, p.Description, p.Filename, ";
            sql         += "p.PictureAddDate, p.PictureUpdateDate ";
            sql += "from Picture p ";
            if (sqlWhereClause.Length > 0)
            {
                sql += "WHERE " + sqlWhereClause + " ";
            }
            sql += "ORDER BY PictureDate, PictureSort";

            SqlConnection cn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr = null;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    pictures.Add(new PictureData(dr));
                }
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return pictures;
        }

        public static string GetSqlSortFieldName(PictureSortField sortField)
        {
            string sortFieldSqlName = "PictureSort";

            if (sortField == PictureSortField.DatePictureAdded)
            {
                sortFieldSqlName = "PictureAddDate";
            }
            else if (sortField == PictureSortField.DatePictureTaken)
            {
                sortFieldSqlName = "PictureDate";
            }
            else if (sortField == PictureSortField.DatePictureUpdated)
            {
                sortFieldSqlName = "PictureUpdateDate";
            }

            return sortFieldSqlName;
        }

        public DataSet GetPictures(int categoryId, int startRecord, int returnCount, 
            int maxWidth, int maxHeight, PictureSortField sortField, PictureSortOrder sortOrder, ref int totalCount)
		{
			// init connection and command to get pictures
			SqlConnection cn  = new SqlConnection(connectionString);

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_Category_GetPictures", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

            string sortFieldSqlName = GetSqlSortFieldName(sortField);

            if (sortOrder == PictureSortOrder.SortDescending)
            {
                sortFieldSqlName += " DESC";
            }

            // set up params on the SP
			int personId = PicContext.Current.CurrentUser.Id;
			daPics.SelectCommand.Parameters.Add("@CategoryID", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@StartRecord", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@ReturnCount", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@PersonID", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", SqlDbType.Int);
            daPics.SelectCommand.Parameters.Add("@SortFieldName", SqlDbType.NVarChar, 50);
            daPics.SelectCommand.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			daPics.SelectCommand.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

			daPics.SelectCommand.Parameters["@CategoryID"].Value = categoryId;
			daPics.SelectCommand.Parameters["@StartRecord"].Value = startRecord;
			daPics.SelectCommand.Parameters["@ReturnCount"].Value = returnCount;
			daPics.SelectCommand.Parameters["@PersonID"].Value = personId;
			daPics.SelectCommand.Parameters["@MaxWidth"].Value = maxWidth;
			daPics.SelectCommand.Parameters["@MaxHeight"].Value = maxHeight;
			daPics.SelectCommand.Parameters["@SortFieldName"].Value = sortFieldSqlName;

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			totalCount = (int) daPics.SelectCommand.Parameters["@TotalCount"].Value;

			return dsPics;
		}


		public List<PersonGroup> GetPictureGroups(int pictureId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("sp_Picture_GetGruops", cn);
            SqlDataReader dr = null;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters["@pictureId"].Value	= pictureId;

            List<PersonGroup> list = new List<PersonGroup>();

			try
			{
				cn.Open();

                dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                while (dr.Read())
                {
                    PersonGroup group = new PersonGroup(
                        dr.GetInt32(1),
                        dr.GetString(0));
                    list.Add(group);
                }                
			}
			finally
			{
                if (dr != null)
                {
                    dr.Close();
                }
				if (cn.State != ConnectionState.Closed)
				{
					cn.Close();
				}
			}

			return list;
		}

		public List<Category> GetPictureCategories(int pictureId)
		{
            List<Category> categories = new List<Category>();

            SqlConnection cn	= new SqlConnection(connectionString);
			SqlCommand cmd      = new SqlCommand("sp_Picture_GetCategories", cn);
			cmd.CommandType     = CommandType.StoredProcedure;
			cmd.Parameters.Add("@pictureId", SqlDbType.Int);
			cmd.Parameters["@pictureId"].Value	= pictureId;

			try
			{
				cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Category category = new Category(dr, false);
                    categories.Add(category);
                }
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

			return categories;
		}

		public DataSet GetPicturePeople(int pictureId)
		{
			SqlConnection cn	= new SqlConnection(connectionString);
			SqlDataAdapter da	= new SqlDataAdapter("sp_Picture_GetPeople", cn);
			DataSet ds			= new DataSet();
			da.SelectCommand.CommandType = CommandType.StoredProcedure;
			da.SelectCommand.Parameters.Add("@pictureId", SqlDbType.Int);
			da.SelectCommand.Parameters["@pictureId"].Value	= pictureId;

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


        [Obsolete]
        public DataSetPicture RandomImageData()
		{
			return RandomImageData(PicContext.Current.CurrentUser.Id);
		}

        [Obsolete]
		public DataSetPicture RandomImageData(int personId)
		{

			SqlConnection cn  = new SqlConnection(connectionString);
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_RandomPicture", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@PersonID", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", SqlDbType.Int);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", SqlDbType.Int);

			daPics.SelectCommand.Parameters["@PersonID"].Value = personId;
			daPics.SelectCommand.Parameters["@MaxWidth"].Value = 125;
			daPics.SelectCommand.Parameters["@MaxHeight"].Value = 125;

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSetPicture dsPics = new DataSetPicture();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();
            
			return dsPics;
		}

        public DateCollection GetPictureDates()
        {
            return GetDates("p_GetPictureDates");
        }

        public DateCollection GetPictureAddedDates()
        {
            return GetDates("p_GetPictureAddedDates");
        }

        private DateCollection GetDates(string proc)
        {
            SqlConnection cn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand(proc, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            DateCollection col = new DateCollection();

            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                col.Add(dr.GetInt32(0), dr.GetInt32(1), dr.GetInt32(2));
            }
            dr.Close();
            cn.Close();

            return col;

        }

    }

    public class PictureCollection : ReadOnlyCollectionBase
    {
        public void Add(PictureData picture)
        {
            base.InnerList.Add(picture);
        }

        public PictureData this[int index]
        {
            get
            {
                return base.InnerList[index] as PictureData;
            }
        }
    }
    
    public class DateCollection : ReadOnlyCollectionBase
    {
        internal void Add(int year, int month, int day)
        {
            DateItem item = new DateItem();
            item.Year = year;
            item.Month = month;
            item.Day = day;
            InnerList.Add(item);
        }

        public DateItem this[int index]
        {
            get
            {
                return this[index] as DateItem;
            }
        }
    }

    public class DateItem
    {
        public int Year;
        public int Month;
        public int Day;
    }



}
