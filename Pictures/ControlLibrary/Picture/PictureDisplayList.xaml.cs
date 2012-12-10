using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace msn2.net.Pictures.Controls
{
    /// <summary>
    /// Interaction logic for PictureDisplayList.xaml
    /// </summary>
    public partial class PictureDisplayList : UserControl, IPictureList
    {
        int pictureSize = 100;

        public PictureDisplayList()
        {
            InitializeComponent();
        }

        #region IPictureList Members

        public Picture GetNextPicture(int pictureId)
        {
            bool found = false;

            foreach (PictureDisplayItem item in this.wrapPanel.Children)
            {
                if (found)
                {
                    return item.Picture;
                }

                if (item.Picture != null && item.Picture.Id == pictureId)
                {
                    found = true;
                }
            }

            return null;
        }

        public Picture GetPreviousPicture(int pictureId)
        {
            Picture last = null;

            foreach (PictureDisplayItem item in this.wrapPanel.Children)
            {
                if (item.Picture != null && item.Picture.Id == pictureId)
                {
                    return last;
                }

                last = item.Picture;
            }

            return null;
        }

        List<PictureDisplayItem> GetItems()
        {
            List<PictureDisplayItem> items = new List<PictureDisplayItem>();

            foreach (PictureDisplayItem item in this.wrapPanel.Children)
            {
                items.Add(item);
            }

            return items;
        }

        public Picture GetSelectedPictureData()
        {
            Picture pic = null;
            List<PictureDisplayItem> items = this.GetItems();

            if (items.Any(i => i.Selected))
            {
                PictureDisplayItem item = items.First(i => i.Selected);
                pic = item.Picture;
            }

            return pic;
        }

        public int ItemCount
        {
            get 
            {
                return this.GetItems().Count;
            }
        }

        public List<int> SelectedItems
        {
            get
            {
                List<int> items = new List<int>();
                List<PictureDisplayItem> pdis = this.GetItems(); 
                foreach (PictureDisplayItem item in pdis.Where(i => i.Selected))
                {
                    items.Add(item.Picture.Id);
                }
                return items;
            }
            set
            {
                List<PictureDisplayItem> items = this.GetItems();
                items.ForEach(i => i.Selected = false);

                foreach (int id in value)
                {
                    foreach (PictureDisplayItem item in items)
                    {
                        if (item.Picture.Id == id)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        public event PictureItemEventHandler ItemSelected;

        public event PictureItemEventHandler ItemUnselected;

        public event EventHandler MultiSelectEnd;

        // TODO: Implement multi select events
        public event EventHandler MultiSelectStart;

        public event PictureItemEventHandler PictureDoubleClick;

        public event PictureItemEventHandler SelectedChanged;

        public void LoadPictures(List<Picture> pictures)
        {
            this.ClearPictures();

            foreach (Picture p in pictures)
            {
                PictureDisplayItem item = new PictureDisplayItem()
                { 
                    Height = this.pictureSize, 
                    Width = this.pictureSize 
                };
                item.SetPicture(p);
                item.ClearSelection += new EventHandler(item_ClearSelection);
                item.SelectedChanged += new EventHandler(item_SelectedChanged);
                item.MouseDoubleClick += new MouseButtonEventHandler(item_MouseDoubleClick);
                this.wrapPanel.Children.Add(item);
            }
        }

        void item_ClearSelection(object sender, EventArgs e)
        {
            this.ClearAll();
        }

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.PictureDoubleClick != null)
            {
                PictureDisplayItem item = (PictureDisplayItem)sender;
                if (item.Picture != null)
                {
                    this.PictureDoubleClick(this, new PictureItemEventArgs(item.Picture));
                }
            }
        }

        void item_SelectedChanged(object sender, EventArgs e)
        {
            PictureDisplayItem item = (PictureDisplayItem)sender;

            if (item.Selected)
            {
                if (this.ItemSelected != null)
                {
                    this.ItemSelected(this, new PictureItemEventArgs(item.Picture));
                }
                if (this.SelectedChanged != null)
                {
                    this.SelectedChanged(this, new PictureItemEventArgs(item.Picture));
                }
            }
            else
            {
                if (this.ItemUnselected != null)
                {
                    this.ItemUnselected(this, new PictureItemEventArgs(item.Picture));
                }
            }
        }

        public void ReleasePicture(int pictureId)
        {
            PictureDisplayItem item = this.GetPictureItem(pictureId);
            item.SetPicture(null);
        }

        public void ReloadPicture(int pictureId)
        {
            PictureDisplayItem item = this.GetPictureItem(pictureId);
            Picture data = PicContext.Current.Clone().PictureManager.GetPicture(pictureId);
            item.SetPicture(data);
        }

        public void Remove(int pictureId)
        {
            PictureDisplayItem item = this.GetPictureItem(pictureId);
            this.wrapPanel.Children.Remove(item);
            item.SetPicture(null);
        }

        public void ClearAll()
        {
            List<PictureDisplayItem> items = this.GetItems();
            items.ForEach(i => i.Selected = false);
        }

        public void SelectAll()
        {
            List<PictureDisplayItem> items = this.GetItems();
            items.ForEach(i => i.Selected = true);
        }

        public void SetImageSize(int squareSize)
        {
            this.pictureSize = squareSize;

            foreach (PictureDisplayItem item in this.wrapPanel.Children)
            {
                item.Height = squareSize;
                item.Width = squareSize;
                item.QueueReloads();
            }
        }

        void ClearPictures()
        {
            foreach (PictureDisplayItem item in this.wrapPanel.Children)
            {
                item.SetPicture(null);
            }
            this.wrapPanel.Children.Clear();
        }

        #endregion

        PictureDisplayItem GetPictureItem(int id)
        {
            PictureDisplayItem item = null;

            foreach (PictureDisplayItem p in this.wrapPanel.Children)
            {
                if (p.Picture != null && p.Picture.Id == id)
                {
                    item = p;
                    break;
                }
            }

            return item;
        }

        private void listView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<PictureDisplayItem> items = this.GetItems();
            if (items.Any(i => i.Selected))
            {
                if (this.PictureDoubleClick != null)
                {
                    PictureDisplayItem item = items.First(i => i.Selected);
                    this.PictureDoubleClick(this, new PictureItemEventArgs(item.Picture));
                }
            }
        }

    }
}
