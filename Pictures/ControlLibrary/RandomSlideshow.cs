using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public class RandomSlideshow: Slideshow
    {
        private List<PictureData> pictures = new List<PictureData>();
        private Timer timer = new Timer();

        public RandomSlideshow():
            base(new PictureControlSettings(), null, null)
        {
            base.getPreviousId = new GetPreviousItemIdDelegate(GetPreviousPicture);
            base.getNextId = new GetNextItemIdDelegate(GetNextPicture);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Interval = 8 * 1000;

            if (PicContext.Current != null)
            {   
                base.SetPicture(PicContext.Current.PictureManager.GetRandomPicture());
                this.timer.Enabled = true;
                this.timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // get next picture
            PictureData picture = GetNextPicture(base.CurrentPicture.Id);
            base.SetPicture(picture);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.timer.Enabled = false;
            this.timer = null;
        }

        private PictureData GetPreviousPicture(int currentPictureId)
        {
            int index = GetPictureIndex(currentPictureId);
            if (index > 0)
            {
                return this.pictures[index];
            }

            return null;
        }

        private PictureData GetNextPicture(int currentPictureId)
        {
            int index = GetPictureIndex(currentPictureId);
            if (index < 0 || index == pictures.Count-1)
            {
                PictureData randomPicture = PicContext.Current.PictureManager.GetRandomPicture();
                pictures.Add(randomPicture);
            }

            return pictures[index + 1];
        }

        private int GetPictureIndex(int pictureId)
        {
            int match = -1;

            for (int index = this.pictures.Count - 1; index >= 0; index--)
            {
                if (this.pictures[index].Id == pictureId)
                {
                    match = index;
                    break;
                }
            }

            return match;
        }
    }
}
