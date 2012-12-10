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
    }
}
