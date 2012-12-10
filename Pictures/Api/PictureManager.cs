using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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
        private PicContext picContext;

        public PictureManager(PicContext picContext)
        {
            this.picContext = picContext;
        }

        public void AddToCategory(int pictureId, int categoryId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_AddToCategory", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@categoryId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@categoryId"].Value = categoryId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }

        public void RemoveFromCategory(int pictureId, int categoryId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_RemoveFromCategory", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@categoryId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@categoryId"].Value = categoryId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }

        public void AddPerson(int pictureId, int personId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_AddPerson", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@personId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@personId"].Value = personId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }

        public void RemovePerson(int pictureId, int personId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_RemovePerson", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@personId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@personId"].Value = personId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }

        public decimal RatePicture(int pictureId, int rating)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
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

            decimal averageRating = (decimal)cmd.Parameters["@averageRating"].Value;
            return averageRating;
        }

        public void AddToSecurityGroup(int pictureId, int groupId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("p_Picture_AddToSecurityGroup", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters.Add("@groupId", SqlDbType.Int);

            cmd.Parameters["@pictureId"].Value = pictureId;
            cmd.Parameters["@groupId"].Value = groupId;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public void RemoveFromSecurityGroup(int pictureId, int groupId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
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
            Picture data = GetPicture(pictureId);

            // Load iamge
            string appPath = PicContext.Current.Config.PictureDirectory;
            if (!appPath.Equals(@"\"))
                appPath = appPath + @"\";

            string file = Path.Combine(appPath, data.Filename);

            using (Image image = Image.FromFile(file))
            {
                image.RotateFlip(rft);
                image.Save(file);
            }

        }

        public Image GetPictureImage(Picture picture, int maxWidth, int maxHeight)
        {
            Image image = this.picContext.PictureCache.GetImage(picture, maxWidth, maxHeight);
            return image;
        }

        public byte? GetUserRating(int pictureId)
        {
            PictureRating rating = this.picContext.DataContext.PictureRatings.FirstOrDefault(
                r => r.PictureId == pictureId && r.PersonId == this.picContext.CurrentUser.Id);
            if (rating == null)
            {
                return null;
            }

            return rating.Rating;
        }

        public Picture GetPicture(int pictureId)
        {
            Picture picture = (from p in this.picContext.DataContext.Pictures
                               where p.PictureID == pictureId && p.PictureGroups.Any(
                                    pg => pg.Group.PersonGroups.Any(
                                        i => i.PersonID == this.picContext.CurrentUser.Id))
                               select p).FirstOrDefault();
            return picture;
        }

        public Picture GetRandomPicture()
        {
            return this.GetRandomPicture(125, 125, @"\", 0);
        }
        
        public Picture GetRandomPicture(int maxWidth, int maxHeight, string path, int overrideGroupId)
        {
            var q = this.picContext.DataContext.GetRandomPicture(
                this.picContext.CurrentUser.Id, maxWidth, maxHeight, path, overrideGroupId);

            Picture picture = q.FirstOrDefault<Picture>();

            return picture;
        }

        public void AddPicture(Picture picture)
        {
            this.picContext.DataContext.Pictures.InsertOnSubmit(picture);
            this.picContext.DataContext.SubmitChanges();
        }

        public void AddPictureCache(Picture picture, PictureCache cache)
        {
            this.picContext.DataContext.PictureCaches.InsertOnSubmit(cache);
            this.picContext.DataContext.SubmitChanges();
        }

        public PictureCache GetPictureCache(int pictureId, int maxWidth, int maxHeight)
        {
            PictureCache pc = this.picContext.DataContext.PictureCaches.FirstOrDefault(
                i => i.PictureID == pictureId && i.MaxWidth == maxWidth && i.MaxHeight == maxHeight);
            return pc;
        }

        public List<Picture> GetPictures(int categoryId, DateTime startTime, DateTime endTime)
        {
            DateTime minDate = DateTime.Parse("1/1/1900");
            DateTime maxDate = DateTime.Parse("1/1/2100");
            if (startTime < minDate)
            {
                startTime = minDate;
            }
            if (endTime > maxDate)
            {
                endTime = maxDate;
            }
            var q = from p in this.picContext.DataContext.Pictures
                    join pc in this.picContext.DataContext.PictureCategories on p.PictureID equals pc.PictureID
                    join pg in this.picContext.DataContext.PictureGroups on p.PictureID equals pg.PictureID
                    join per in this.picContext.DataContext.PersonGroups on pg.GroupID equals per.GroupID
                    join csc in this.picContext.DataContext.CategorySubCategories on pc.CategoryID equals csc.SubCategoryID
                    where csc.CategoryID == categoryId && per.PersonID == this.picContext.CurrentUser.Id
                        && per.PersonID == this.picContext.CurrentUser.Id 
                        && p.PictureDate > startTime && p.PictureDate < endTime
                    orderby p.PictureDate
                    select p;

            return q.Distinct().ToList();
        }

        public List<Picture> GetPicturesByCategory(int categoryId)
        {
            var q = from p in this.picContext.DataContext.Pictures
                    join pc in this.picContext.DataContext.PictureCategories on p.PictureID equals pc.PictureID
                    join pg in this.picContext.DataContext.PictureGroups on p.PictureID equals pg.PictureID
                    join per in this.picContext.DataContext.PersonGroups on pg.GroupID equals per.GroupID
                    where pc.CategoryID == categoryId && per.PersonID == this.picContext.CurrentUser.Id
                    orderby p.PictureDate
                    select p;

            return q.Distinct().ToList();
        }

        public List<Picture> GetPicturesByDate(DateTime fromTime, DateTime toTime)
        {
            var q = from p in this.picContext.DataContext.Pictures
                    join pc in this.picContext.DataContext.PictureCategories on p.PictureID equals pc.PictureID
                    join pg in this.picContext.DataContext.PictureGroups on p.PictureID equals pg.PictureID
                    join per in this.picContext.DataContext.PersonGroups on pg.GroupID equals per.GroupID
                    where per.PersonID == this.picContext.CurrentUser.Id 
                        && p.PictureDate > fromTime && p.PictureDate < toTime
                    select p;

            return q.Distinct().ToList();
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
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);

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

            totalCount = (int)daPics.SelectCommand.Parameters["@TotalCount"].Value;

            return dsPics;
        }

        public List<PersonGroupInfo> GetPictureGroups(int pictureId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_Picture_GetGruops", cn);
            SqlDataReader dr = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters["@pictureId"].Value = pictureId;

            List<PersonGroupInfo> list = new List<PersonGroupInfo>();

            try
            {
                cn.Open();

                dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                while (dr.Read())
                {
                    PersonGroupInfo group = new PersonGroupInfo(
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
            var q = from c in this.picContext.DataContext.Categories
                    join pc in this.picContext.DataContext.PictureCategories on c.Id equals pc.CategoryID
                    where pc.PictureID == pictureId
                    select c;
            return q.ToList();
        }

        public List<PersonInfo> GetPicturePeople(int pictureId)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_Picture_GetPeople", cn);
            SqlDataReader dr = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pictureId", SqlDbType.Int);
            cmd.Parameters["@pictureId"].Value = pictureId;

            List<PersonInfo> people = new List<PersonInfo>();

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string email = (dr.IsDBNull(2) == true ? string.Empty : dr.GetString(2));

                    PersonInfo person = new PersonInfo(
                        dr.GetInt32(1),
                        dr.GetString(0),
                        email);
                    people.Add(person);
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

            return people;
        }

        public List<DateItem> GetPictureDates()
        {
            return GetDates("p_GetPictureDates");
        }

        public List<DateItem> GetPictureAddedDates()
        {
            return GetDates("p_GetPictureAddedDates");
        }

        private List<DateItem> GetDates(string proc)
        {
            SqlConnection cn = new SqlConnection(this.picContext.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand(proc, cn);
            cmd.Parameters.Add("@personId", SqlDbType.Int);
            cmd.Parameters["@personId"].Value = this.picContext.CurrentUser.Id;
            cmd.CommandType = CommandType.StoredProcedure;

            List<DateItem> col = new List<DateItem>();

            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                col.Add(new DateItem() { Year = dr.GetInt32(0), Month = dr.GetInt32(1), Day = dr.GetInt32(2) });
            }
            dr.Close();
            cn.Close();

            return col;

        }

        public IQueryable<Picture> GetPictures()
        {
            return this.picContext.DataContext.Pictures;
        }

        public static string MonthString(int Month)
        {
            switch (Month)
            {
                case 1: { return "January"; }
                case 2: { return "February"; }
                case 3: { return "March"; }
                case 4: { return "April"; }
                case 5: { return "May"; }
                case 6: { return "June"; }
                case 7: { return "July"; }
                case 8: { return "August"; }
                case 9: { return "September"; }
                case 10: { return "October"; }
                case 11: { return "November"; }
                case 12: { return "December"; }
            }
            return "Invalid Month";
        }

    }

    public class DateItem
    {
        public int Year;
        public int Month;
        public int Day;
    }



}
