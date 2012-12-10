using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

namespace msn2.net.Pictures.Controls
{
    public class RandomSlideshow : Slideshow
    {
        private List<Picture> pictures = new List<Picture>();
        protected Timer timer = new Timer();
        private Label errorLabel = new Label();
        public string Path { get; set; }
        public int GroupId { get; set; }

        public RandomSlideshow() :
            base(new PictureControlSettings(), null, null)
        {
            base.getPreviousId = new GetPreviousItemIdDelegate(GetPreviousPicture);
            base.getNextId = new GetNextItemIdDelegate(GetNextPicture);

            this.Path = @"\";
            this.GroupId = 0;

            this.errorLabel = new Label();
            this.errorLabel.ForeColor = Color.Red;
            this.errorLabel.Location = new Point(20, 20);
            this.errorLabel.Font = new Font("Calibri", 14, FontStyle.Bold);
            this.errorLabel.BackColor = Color.Transparent;
            this.errorLabel.AutoSize = true;
            this.Controls.Add(this.errorLabel);

            string interval = ConfigurationSettings.AppSettings["timerInterval"];
            if (interval != null)
            {
                this.timer.Interval = int.Parse(interval) * 1000;
            }
            else
            {
                this.timer.Interval = 10 * 1000;
            }
        }

        public int Interval
        {
            get
            {
                return this.timer.Interval / 1000;
            }
            set
            {
                this.timer.Interval = value * 1000;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.timer.Tick += new EventHandler(timer_Tick);

            if (PicContext.Current != null)
            {
                Picture pic = this.GetRandomPicture();
                if (pic != null)
                {
                    base.SetPicture(pic.Id);
                }
                this.timer.Enabled = true;
                this.timer.Start();
            }
        }

        Picture GetRandomPicture()
        {
            return PicContext.Current.PictureManager.GetRandomPicture(750, 700, this.Path, this.GroupId);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Picture picture = null;

            try
            {
                // get next picture
                picture = GetNextPicture(base.CurrentPicture.Id);
                if (picture != null)
                {
                    base.SetPicture(picture);
                    this.errorLabel.Visible = false;
                }
                else
                {
                    this.errorLabel.Visible = true;
                    this.errorLabel.BringToFront();
                    this.errorLabel.Text = "Unable to find another picture.";
                }
            }
            catch (Exception ex)
            {
                this.errorLabel.BringToFront();
                this.errorLabel.Visible = true;
                if (picture == null)
                {
                    this.errorLabel.Text = "Error loading next picture: " + ex.Message;
                }
                else
                {
                    this.errorLabel.Text = "Error setting pic " + picture.Id.ToString() + ": " + ex.Message;
                }
                this.errorLabel.Refresh();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.timer.Enabled = false;
            this.timer = null;
        }

        private Picture GetPreviousPicture(int currentPictureId)
        {
            int index = GetPictureIndex(currentPictureId);
            if (index > 0)
            {
                return this.pictures[index - 1];
            }

            return null;
        }

        private Picture GetNextPicture(int currentPictureId)
        {
            int index = GetPictureIndex(currentPictureId);
            int retryCount = 100;
            if (index < 0 || index == pictures.Count - 1)
            {
                Picture addedPicture = null;
                do
                {
                    Picture randomPicture = this.GetRandomPicture();
                    if (randomPicture != null)
                    {
                        addedPicture = pictures.Find(p => p.Id == randomPicture.Id);

                        if (addedPicture == null)
                        {
                            pictures.Add(randomPicture);
                        }
                    }

                    retryCount--;

                } while (addedPicture != null && retryCount > 0);
            }

            Picture pic = null;
            if (retryCount == 0)
            {
                this.pictures.Clear();
            }
            else
            {
                pic = pictures[index + 1];
            }
            return pic;
        }


        private int GetPictureIndex(int pictureId)
        {
            int match = -1;

            for (int index = this.pictures.Count - 1; index >= 0; index--)
            {
                if (this.pictures[index] != null && this.pictures[index].Id == pictureId)
                {
                    match = index;
                    break;
                }
            }

            return match;
        }
    }
}
