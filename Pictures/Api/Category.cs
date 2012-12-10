#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace msn2.net.Pictures
{
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

        internal Category(SqlDataReader dr, bool includeDates)
        {
            CategoryId = (int)dr["CategoryId"];
            CategoryName = dr["CategoryName"].ToString();
            CategoryParentId = (int)dr["CategoryParentId"];
            PictureId = (int)dr["PictureId"];
            if (dr["CategoryDescription"] != null)
            {
                Description = dr["CategoryDescription"].ToString();
            }
            parentCategoryId = (int)dr["CategoryParentId"];

            if (includeDates)
            {
                if (dr["FromDate"] != System.DBNull.Value)
                {
                    FromDate = Convert.ToDateTime(dr["FromDate"]);
                }
                if (dr["ToDate"] != System.DBNull.Value)
                {
                    ToDate = Convert.ToDateTime(dr["ToDate"]);
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

}
