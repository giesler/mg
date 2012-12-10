﻿#region Using directives

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
     //       this.DoubleBuffered = true;
        }

        public void LoadPictures(List<Picture> pictures)
        {
            this.SuspendLayout();

            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = string.Format("loading {0} pictures...", pictures.Count);
            this.Controls.Add(label);

            flowLayoutPanel1.Visible = false;

            this.ResumeLayout(true);

            flowLayoutPanel1.SuspendLayout();

            selectedItems.Clear();

            flowLayoutPanel1.Controls.Clear();

            foreach (Picture picture in pictures)
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
            flowLayoutPanel1.Visible = true;

            this.Controls.Remove(label);
        }

        public int ItemCount
        {
            get
            {
                return this.flowLayoutPanel1.Controls.Count;
            }
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

        public void ReleasePicture(int pictureId)
        {
            PictureItem item = FindItem(pictureId);
            item.ReleaseImages();
        }

        public void ReloadPicture(int pictureId)
        {
            PictureItem item = FindItem(pictureId);
            Picture data = PicContext.Current.Clone().PictureManager.GetPicture(pictureId);
            item.SetPicture(data);
        }

        public Picture GetSelectedPictureData()
        {
            Picture val = null;

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

        public Picture GetNextPicture(int pictureId)
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

        public Picture GetPreviousPicture(int pictureId)
        {
            Picture lastPicture = null;

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
                    if (SelectedChanged != null)
                    {
                        this.SelectedChanged(this, new PictureItemEventArgs(item.Picture));
                    }

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
            bool selected = item.Selected;

            MouseEventArgs eArgs = (MouseEventArgs)e;

            if (eArgs.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys != Keys.Control
                    && Control.ModifierKeys != Keys.Shift)
                {
                    this.ClearAll(item);
                }

                if (selected == false)
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
                else
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
        private Picture picture;

        internal PictureItemEventArgs(Picture picture)
        {
            this.picture = picture;
        }

        public Picture Picture
        {
            get
            {
                return picture;
            }
        }

    }
}
