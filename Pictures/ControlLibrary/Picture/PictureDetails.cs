using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public partial class PictureDetails : PropertyForm
    {
        Picture picture = null;
        PicContext context = null;

        public PictureDetails(PicContext context)
        {
            this.context = context;

            InitializeComponent();

            this.title.LostFocus += new EventHandler(controlLostFocus);
            this.dateTaken.LostFocus += new EventHandler(controlLostFocus);
            this.description.LostFocus += new EventHandler(controlLostFocus);
        }

        void controlLostFocus(object sender, EventArgs e)
        {
            bool changed = false;

            if (this.picture != null)
            {
                if (this.title.Text != this.picture.Title)
                {
                    this.picture.Title = this.title.Text;
                    changed = true;
                }
                //if (this.dateTaken.Value != this.picture.PictureDate)
                //{
                //    this.picture.PictureDate = this.dateTaken.Value;
                //    changed = true;
                //}
                if (this.description.Text != this.picture.Description)
                {
                    this.picture.Description = this.description.Text;
                    changed = true;
                }
                if (changed)
                {
                    this.context.SubmitChanges();
                }
            }
        }

        public void SetPicture(int pictureId)
        {
            this.picture = this.context.PictureManager.GetPicture(pictureId);

            if (this.picture != null)
            {
                this.title.Text = this.picture.Title;
                this.dateTaken.Value = this.picture.PictureDate;
                this.description.Text = this.picture.Description;
            }
            else
            {
                this.title.Text = "";
                this.dateTaken.Value = DateTime.Now;
                this.description.Text = "";
            }
        }
    }
}
