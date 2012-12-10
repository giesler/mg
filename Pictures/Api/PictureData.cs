#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace msn2.net.Pictures
{
    public class PictureData
    {
        internal PictureData(SqlDataReader dr)
        {
            this.id = dr.GetInt32(1);
            this.dateTaken = dr.GetDateTime(2);
            this.title = dr.GetString(3);
            this.description = dr.GetString(4);
            this.fileName = dr.GetString(5);
            this.dateAdded = dr.GetDateTime(6);
            this.dateUpdated = dr.GetDateTime(7);

            if (dr.FieldCount > 8)
            {
                if (dr.IsDBNull(8) == false)
                {
                    this.averageRating = dr.GetDecimal(8);
                }

                if (dr.IsDBNull(9) == false)
                {
                    this.userRating = dr.GetByte(9);
                }
            }

            dirty = false;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Filename
        {
            get
            {
                return fileName;
            }
        }

        public DateTime DateTaken
        {
            get
            {
                return dateTaken;
            }
            set
            {
                dateTaken = value;
                dirty = true;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                dirty = true;
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
                dirty = true;
            }
        }

        public DateTime DateAdded
        {
            get
            {
                return dateAdded;
            }
        }

        public DateTime DateUpdated
        {
            get
            {
                return dateUpdated;
            }
        }

		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

        public int UserRating
        {
            get
            {
                return this.userRating;
            }
        }

        public decimal AverageRating
        {
            get
            {
                return this.averageRating;
            }
        }

        #region Declares

        private bool dirty;
        private int id;
        private string fileName;
        private DateTime dateTaken;
        private string title;
        private string description;
        private DateTime dateAdded;
        private DateTime dateUpdated;
        private int userRating;
        private decimal averageRating;

        #endregion
    }
}
