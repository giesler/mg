using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace msn2.net.Pictures
{
    /// <summary>
    /// Summary description for CategoryManager.
    /// </summary>
    public class CategoryManager
    {
        private PicContext context;

        internal CategoryManager(PicContext context)
        {
            this.context = context;
        }

        public Category GetCategory(int categoryId)
        {
            return this.context.DataContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public Category GetRootCategory()
        {
            return this.GetCategory(1);
        }

        public List<Category> GetChildrenCategories(int categoryId)
        {
            var q = from c in this.context.DataContext.Categories
                    where c.ParentId == categoryId && c.Id != categoryId &&
                        (from cg in c.CategoryGroups
                         where (from pg in cg.Group.PersonGroups
                                where pg.PersonID == this.context.CurrentUser.Id
                                select pg).Any()
                         select cg).Any()
                    orderby c.Name
                    select c;
            return q.ToList();
        }

        public List<Category> GetCategoriesWithPictures(int categoryId)
        {
            var q = from c in this.context.DataContext.Categories
                    join cSubCat in this.context.DataContext.CategorySubCategories on c.Id equals cSubCat.CategoryID
                    join cSubPic in this.context.DataContext.PictureCategories on cSubCat.SubCategoryID equals cSubPic.CategoryID
                    where c.ParentId == categoryId && c.Id != categoryId
                        &&
                        (from cg in c.CategoryGroups
                         where (from pg in cg.Group.PersonGroups
                                where pg.PersonID == this.context.CurrentUser.Id
                                select pg).Any()
                         select cg).Any()
                        &&
                        (from pg in this.context.DataContext.PictureGroups
                         where pg.PictureID == cSubPic.PictureID
                           && pg.Group.PersonGroups.Any(p => p.PersonID == this.context.CurrentUser.Id)
                         select pg).Any()
                    select c;

            return q.Distinct().OrderBy(c => c.Name).ToList();
        }

        public int PictureCount(int categoryId)
        {
            return PictureCount(categoryId, false);
        }

        public int PictureCount(int categoryId, bool recursive)
        {
            Category category = this.GetCategory(categoryId);

            var q = from pc in this.context.DataContext.PictureCategories
                    join c in this.context.DataContext.Categories on pc.CategoryID equals c.Id
                    where ((recursive == false && pc.CategoryID == categoryId) || (recursive == true && c.Path.StartsWith(category.Path))) &&
                          (from cg in c.CategoryGroups
                           where cg.Group.PersonGroups.Any(p => p.PersonID == this.context.CurrentUser.Id)
                           select cg).Any()
                           &&
                           (from picsec in this.context.DataContext.PictureGroups
                            where picsec.Group.PersonGroups.Any(p => p.PersonID == this.context.CurrentUser.Id)
                                && picsec.PictureID == pc.PictureID
                            select picsec).Any()
                            && pc.Picture.Publish == true
                    select pc;

            return q.Count();
        }

        public int CategoryCount(int categoryId)
        {
            return CategoryCount(categoryId, false);
        }

        public int CategoryCount(int categoryId, bool recursive)
        {
            Category category = this.GetCategory(categoryId);

            var q = from c in this.context.DataContext.Categories
                    where c.Id != categoryId &&
                           ((recursive && c.Path.StartsWith(category.Path)) || (recursive == false && c.Id == categoryId)) &&
                           (from pc in c.PictureCategories
                            join cg in c.CategoryGroups on pc.CategoryID equals cg.CategoryID
                            where cg.Group.PersonGroups.Any(p => p.PersonID == this.context.CurrentUser.Id)
                                && (from picgrp in this.context.DataContext.PictureGroups
                                    where picgrp.Group.PersonGroups.Any(p => p.PersonID == this.context.CurrentUser.Id)
                                        && picgrp.PictureID == pc.PictureID
                                    select picgrp).Any()
                            select pc).Any()
                    select c;

            return q.Count();
        }

        public void SetCategoryPictureId(int categoryId, int pictureId)
        {
            Category category = this.GetCategory(categoryId);
            category.PictureId = pictureId;
            this.context.SubmitChanges();
        }

        public void PublishCategory(int categoryId)
        {
            RecentCategory recent = new RecentCategory();
            recent.CategoryId = categoryId;
            recent.Date = DateTime.Now;
            this.context.DataContext.RecentCategories.InsertOnSubmit(recent);
            this.context.SubmitChanges();
        }

        public List<Category> RecentCategorires()
        {
            var q = from c in this.context.DataContext.Categories
                    join r in this.context.DataContext.RecentCategories on c.Id equals r.CategoryId
                    where (from cg in c.CategoryGroups
                           where (from pg in cg.Group.PersonGroups
                                  where pg.PersonID == this.context.CurrentUser.Id
                                  select pg).Any()
                           select cg).Any()
                    orderby r.Date descending
                    select c;

            return q.Take(10).ToList();
        }

        public string[] GetCategoryGroups(int categoryId)
        {
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("sp_CategoryManager_GetGruops", cn);
            DataSet ds = new DataSet();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@categoryId", SqlDbType.Int);
            da.SelectCommand.Parameters["@categoryId"].Value = categoryId;

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

        public void DeleteCategory(int categoryId)
        {
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("up_Category_Delete", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@categoryId", SqlDbType.Int);
            cmd.Parameters["@categoryId"].Value = categoryId;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

        }

        public void MoveCategory(int categoryId, int newParentId)
        {
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("up_CategoryManager_MoveCategory", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@categoryId", SqlDbType.Int);
            cmd.Parameters["@categoryId"].Value = categoryId;
            cmd.Parameters.Add("@newParentCategoryId", SqlDbType.Int);
            cmd.Parameters["@newParentCategoryId"].Value = newParentId;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Message.IndexOf("cannot move") >= 0)
                {
                    throw new ApplicationException(ex.Message);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        public void ReloadCategoryCache()
        {
            this.ReloadCategoryCache(this.GetRootCategory().Id);
        }

        void ReloadCategoryCache(int categoryId)
        {
            List<Category> subCats = this.GetChildrenCategories(categoryId);
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_CategorySubCategoryUpdate", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@categoryId", SqlDbType.Int);

            cn.Open();
            foreach (Category info in subCats)
            {
                cmd.Parameters["@categoryId"].Value = info.Id;
                cmd.ExecuteNonQuery();
            }
            cn.Close();

            foreach (Category info in subCats)
            {
                this.ReloadCategoryCache(info.Id);
            }
        }

        public CategoryStats GetCategoryStats(int categoryId)
        {
            Category category = this.GetCategory(categoryId);

            var q = from pc in this.context.DataContext.PictureCategories
                    join c in this.context.DataContext.Categories on pc.CategoryID equals c.Id
                    join p in this.context.DataContext.Pictures on pc.PictureID equals p.PictureID
                    where c.Path.StartsWith(category.Path) && c.Publish == true &&
                          (from cg in c.CategoryGroups
                           where (from pg in cg.Group.PersonGroups
                                  where pg.PersonID == this.context.CurrentUser.Id
                                  select pg).Any()
                           select cg).Any()
                    orderby p.PictureDate
                    select p;

            CategoryStats stats = new CategoryStats();
            stats.PictureCount = q.Count();
            if (stats.PictureCount > 0)
            {
                stats.StartTime = q.First().PictureDate;
                stats.EndTime = q.Skip(stats.PictureCount - 1).First().PictureDate;
            }
            return stats;
        }

        public void Add(Category category)
        {
            this.context.DataContext.Categories.InsertOnSubmit(category);
            this.context.DataContext.SubmitChanges();
        }
    }

    public class CategoryStats
    {
        public int PictureCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

