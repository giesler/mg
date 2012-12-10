using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.Pictures
{
    public partial class Picture
    {
        public int Id
        {
            get
            {
                return this.PictureID;
            }
        }

        partial void OnCreated()
        {
            this.Description = string.Empty;
            this.AverageRating = 50;
            this.Publish = true;
        }
    }
}
