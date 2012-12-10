#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    partial class PictureProperties : PropertyForm
    {
        public PictureProperties()
        {
            InitializeComponent();
        }

        public void SetPicture(Picture picture)
        {
            this.multiPictureEdit1.ClearPictures();
            this.multiPictureEdit1.AddPicture(picture);
        }

        public MultiPictureEdit Editor
        {
            get
            {
                return this.multiPictureEdit1;
            }
        }
    }
}