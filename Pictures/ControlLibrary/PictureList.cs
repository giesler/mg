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

        public void LoadPictures(DataSet ds)
        {
            selectedItems.Clear();
            flowLayoutPanel1.Controls.Clear();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int pictureId = (int)row["PictureId"];
                PictureItem pi = new PictureItem(pictureId);
                pi.DoubleClick += new EventHandler(pi_DoubleClick);
                pi.Click    += new EventHandler(pi_Click);
                pi.DrawBorder = false;
                pi.Width = imageSize;
                pi.Height = imageSize;
                flowLayoutPanel1.Controls.Add(pi);
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

        public int GetNextPicture(int pictureId)
        {
            bool found = false;

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (found)
                {
                    return item.PictureId;
                }

                if (item.PictureId == pictureId)
                {
                    found = true;
                }
            }

            return 0;
        }

        public int GetPreviousPicture(int pictureId)
        {
            int lastId = 0;

            foreach (PictureItem item in flowLayoutPanel1.Controls)
            {
                if (item.PictureId == pictureId)
                {
                    return lastId;
                }
                lastId = item.PictureId;
            }

            return 0;
        }

        void pi_DoubleClick(object sender, EventArgs e)
        {
            if (null != PictureDoubleClick)
            {
                PictureItem item = sender as PictureItem;
                PictureDoubleClick(this, new PictureItemEventArgs(item.PictureId));
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
                        ItemUnselected(this, new PictureItemEventArgs(item.PictureId));
                    }
                }
                item.Dispose();

                flowLayoutPanel1.Controls.Remove(item);
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
                        ItemSelected(this, new PictureItemEventArgs(item.PictureId));
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
                item.Selected = false;
                if (selectedItems.Contains(item.PictureId))
                {
                    selectedItems.Remove(item.PictureId);

                    if (null != ItemUnselected)
                    {
                        ItemUnselected(this, new PictureItemEventArgs(item.PictureId));
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
                    ItemSelected(this, new PictureItemEventArgs(item.PictureId));

                }
                if (null != SelectedChanged)
                {
                    SelectedChanged(this, new PictureItemEventArgs(item.PictureId));
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
                        ItemUnselected(this, new PictureItemEventArgs(item.PictureId));
                    }
                    if (null != SelectedChanged)
                    {
                        SelectedChanged(this, new PictureItemEventArgs(item.PictureId));
                    }

                }
            }

        }

    }

    public delegate void PictureItemEventHandler(object sender, PictureItemEventArgs e);

    public class PictureItemEventArgs : EventArgs
    {
        internal PictureItemEventArgs(int pictureId)
        {
            this.pictureId = pictureId;
        }

        private int pictureId;

        public int PictureId
        {
            get
            {
                return pictureId;
            }
        }

    }
}
