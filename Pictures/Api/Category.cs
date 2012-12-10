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
		private int categoryId;

		public int CategoryId
		{
			get { return categoryId; }
		}

		private string name;

		private int parentId;

		public int ParentId
		{
			get
			{
				return this.parentId;
			}
		}

        private string description;
        private int pictureId;

		public int PictureId
		{
			get
			{
				return this.pictureId;
			}
		}

		private DateTime fromDate;
		private DateTime toDate;

		public DateTime FromDate
		{
			get
			{
				return this.fromDate;
			}
		}

		public DateTime ToDate
		{
			get
			{
				return this.toDate;
			}
		}

        private string path;
		private bool dirty;

        internal Category()
        {
        }

        internal Category(SqlDataReader dr, bool includeDates)
        {
            this.categoryId = (int)dr["CategoryId"];
            this.name = dr["CategoryName"].ToString();
            this.parentId = (int)dr["CategoryParentId"];
            this.pictureId = (int)dr["PictureId"];
            if (dr["CategoryDescription"] != null)
            {
                this.description = dr["CategoryDescription"].ToString();
            }
            path = dr["CategoryPath"].ToString();

            if (includeDates)
            {
                if (dr["FromDate"] != System.DBNull.Value)
                {
                    this.fromDate = Convert.ToDateTime(dr["FromDate"]);
                }
                if (dr["ToDate"] != System.DBNull.Value)
                {
                    this.toDate = Convert.ToDateTime(dr["ToDate"]);
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

        public string Path
        {
            get
            {
                return this.path;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
				this.name = value;
				this.dirty = true;
            }
        }

		public override string ToString()
        {
            return this.name;
        }
    }

}
