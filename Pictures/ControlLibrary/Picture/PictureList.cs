#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class PictureList : UserControl
    {
        private List<int> selectedItems;
        private int imageSize = 100;

        public PictureList()
        {
            InitializeComponent();
            selectedItems = new List<int>();
            this.DoubleBuffered = true;
        }

        public void LoadPictures(PictureCollection pictures)
        {
            flowLayoutPanel1.SuspendLayout();

            selectedItems.Clear();
            flowLayoutPanel1.Controls.Clear();

            flowLayoutPanel1.ResumeLayout(true);

            flowLayoutPanel1.SuspendLayout();

            foreach (PictureData picture in pictures)
            {
                PictureItem pi = new PictureItem(picture);
                pi.DoubleClick += new EventHandler(pi_DoubleClick);
                pi.Click    += new EventHandler(pi_Click);
                pi.DrawBorder = false;
                pi.Width = imageSize;
                pi.Height = imageSize;
                flowLayoutPanel1.Controls.Add(pi);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        public List<int> SelectedItems
        {
            get
            {
                return selectedItems;
            }
            set
            {
                ClearSelections();
                SelectItems(value);
            }
        }

        private void SelectItems(List<int> items)
        {
            foreach (int id in items)
            {
                PictureItem item = FindItem(id);
                if (null != item)
                {
                    item.Selected = true;
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            this.ClearSelections();
        }

        private PictureItem FindItem(int pictureId)
        {
            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (item.PictureId == pictureId)
                {
                    return item;
                }
            }

            return null;
        }

        public void ReloadPicture(int pictureId)
        {
            PictureItem item = FindItem(pictureId);
            PictureData data = PicContext.Current.PictureManager.GetPicture(pictureId);
            item.SetPicture(data);
        }

        public PictureData GetSelectedPictureData()
        {
            PictureData val = null;

            if (this.selectedItems.Count > 0)
            {
                PictureItem item = this.FindItem(this.selectedItems[0]);
                val = item.Picture;
            }

            return val;
        }

        private void ClearSelections()
        {
            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                }
            }
        }

        public event PictureItemEventHandler SelectedChanged;
        public event PictureItemEventHandler PictureDoubleClick;
        public event PictureItemEventHandler ItemSelected;
        public event PictureItemEventHandler ItemUnselected;

        public PictureData GetNextPicture(int pictureId)
        {
            bool found = false;

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (found)
                {
                    return item.Picture;
                }

                if (item.PictureId == pictureId)
                {
                    found = true;
                }
            }

            return null;
        }

        public PictureData GetPreviousPicture(int pictureId)
        {
            PictureData lastPicture = null;

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (item.PictureId == pictureId)
                {
                    return lastPicture;
                }
                lastPicture = item.Picture;
            }

            return null;
        }

        void pi_DoubleClick(object sender, EventArgs e)
        {
            if (null != PictureDoubleClick)
            {
                PictureItem item = sender as PictureItem;
                PictureDoubleClick(this, new PictureItemEventArgs(item.Picture));
            }
        }

        public void Remove(int pictureId)
        {
            PictureItem item = FindItem(pictureId);
            if (null != item)
            {
                if (item.Selected)
                {
                    if (null != ItemUnselected)
                    {
                        ItemUnselected(this, new PictureItemEventArgs(item.Picture));
                    }
                }
                item.Dispose();

                flowLayoutPanel1.Controls.Remove(item);
                flowLayoutPanel1.Refresh();
            }
        }

        public void SelectAll()
        {
            if (null != MultiSelectStart)
            {
                MultiSelectStart(this, EventArgs.Empty);
            }

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                item.Selected = true;
                if (!selectedItems.Contains(item.PictureId))
                {
                    selectedItems.Add(item.PictureId);

                    if (null != ItemSelected)
                    {
                        ItemSelected(this, new PictureItemEventArgs(item.Picture));
                    }
                }
            }

            if (null != MultiSelectEnd)
            {
                MultiSelectEnd(this, EventArgs.Empty);
            }
        }

        public event EventHandler MultiSelectStart;
        public event EventHandler MultiSelectEnd;

        public void ClearAll()
        {
            ClearAll(null);
        }

        private void ClearAll(PictureItem skipItem)
        {
            if (null != MultiSelectStart)
            {
                MultiSelectStart(this, EventArgs.Empty);
            }

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (null != skipItem && item == skipItem)
                {
                    continue;
                }
                if (item.Selected)
                {
                    item.Selected = false;
                    if (selectedItems.Contains(item.PictureId))
                    {
                        selectedItems.Remove(item.PictureId);

                        if (null != ItemUnselected)
                        {
                            ItemUnselected(this, new PictureItemEventArgs(item.Picture));
                        }
                    }
                }
            }

            if (null != MultiSelectEnd)
            {
                MultiSelectEnd(this, EventArgs.Empty);
            }
        }

        public void SetImageSize(int squareSize)
        {
            imageSize = squareSize;
            this.flowLayoutPanel1.SuspendLayout();
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                c.Width = squareSize;
                c.Height = squareSize;
            }
            this.flowLayoutPanel1.ResumeLayout(true);
            this.flowLayoutPanel1.Refresh();
        }

        void pi_Click(object sender, EventArgs e)
        {
            PictureItem item = sender as PictureItem;

            if (Control.ModifierKeys != Keys.Control
                && Control.ModifierKeys != Keys.Shift)
            {
                this.ClearAll(item);
            }

            if (!item.Selected)
            {
                item.Selected = true;

                selectedItems.Add(item.PictureId);
                if (null != ItemSelected)
                {
                    ItemSelected(this, new PictureItemEventArgs(item.Picture));
                }
                if (null != SelectedChanged)
                {
                    SelectedChanged(this, new PictureItemEventArgs(item.Picture));
                }
            }
            else if (item.Selected)
            {
                if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Shift)
                {
                    item.Selected = false;

                    selectedItems.Remove(item.PictureId);

                    if (null != ItemUnselected)
                    {
                        ItemUnselected(this, new PictureItemEventArgs(item.Picture));
                    }
                    if (null != SelectedChanged)
                    {
                        SelectedChanged(this, new PictureItemEventArgs(item.Picture));
                    }

                }
            }

        }

    }

    public delegate void PictureItemEventHandler(object sender, PictureItemEventArgs e);

    public class PictureItemEventArgs : EventArgs
    {
        private PictureData picture;

        internal PictureItemEventArgs(PictureData picture)
        {
            this.picture = picture;
        }

        public PictureData Picture
        {
            get
            {
                return picture;
            }
        }

    }
}
