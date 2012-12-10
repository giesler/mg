#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class MultiPictureEdit : UserControl
    {
        private List<int> pictures;

        public MultiPictureEdit()
        {
            InitializeComponent();
            pictures = new List<int>();

            description.DisplayMultipleItems = false;
            description.MultiLine = true;
            description.AcceptsReturn = true;

            UpdateControls();
        }

        public void ClearPictures()
        {
            title.FinishEdit();
            description.FinishEdit();
            dateTaken.FinishEdit();

            pictures.Clear();
            title.ClearItems();
            description.ClearItems();

            UpdateControls();
        }

        public void AddPicture(int pictureId)
        {
            pictures.Add(pictureId);

            PictureData data = PicContext.Current.PictureManager.GetPicture(pictureId);
            title.AddItem(pictureId, data.Title);
            description.AddItem(pictureId, data.Description);
            dateTaken.AddItem(pictureId, data.DateTaken);

            UpdateControls();
        }

        public void RemovePicture(int pictureId)
        {
            pictures.Remove(pictureId);
            title.RemoveItem(pictureId);
            description.RemoveItem(pictureId);
            dateTaken.RemoveItem(pictureId);

            UpdateControls();
        }

        private void UpdateControls()
        {
            Color labelColor = (pictures.Count > 0 ? SystemColors.ControlText : SystemColors.GrayText);
            titleLabel.ForeColor = labelColor;
            descriptionLabel.ForeColor = labelColor;
            labelDateTaken.ForeColor = labelColor;

        }

        private void title_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the picture title
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.Title = e.NewValue;
            PicContext.Current.PictureManager.Save(data);
        }

        private void description_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the description
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.Description = e.NewValue;
            PicContext.Current.PictureManager.Save(data);
        }

        private void dateTaken_DateTimeItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.DateTimeItemChangedEventArgs e)
        {
            // Update the date taken
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.DateTaken = dateTaken.Value;
            PicContext.Current.PictureManager.Save(data);
        }

    }
}
