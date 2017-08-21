#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace msn2.net.Pictures.Controls
{
    public class CommonImages
    {
        private static Bitmap folder;

        public static Bitmap Folder
        {
            get
            {
                if (folder == null)
                {
                    folder = new Bitmap(typeof(Resources.ResourceLocator), "folder.ico");
                }
                return folder;
            }
        }

        private static Bitmap calendar;

        public static Bitmap Calendar
        {
            get
            {
                if (calendar == null)
                {
                    calendar = new Bitmap(typeof(Resources.ResourceLocator), "calendar.ico");
                } 
                return calendar;
            }
        }


        private static Bitmap refresh;

        public static Bitmap Refresh
        {
            get
            {
                if (refresh == null)
                {
                    refresh = new Bitmap(typeof(Resources.ResourceLocator), "refresh.ico");
                }
                return refresh;
            }
        }



        private static Bitmap error;

        public static Bitmap Error
        {
            get
            {
                if (error == null)
                {
                    error = new Bitmap(typeof(Resources.ResourceLocator), "error.ico");
                }
                return error;
            }
        }
    }
}
