﻿using System;
using System.Collections.Generic;
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

namespace msn2.net.Pictures.Controls.Picture
{
    /// <summary>
    /// Interaction logic for PictureFlowList.xaml
    /// </summary>
    public partial class PictureFlowList : UserControl
    {
        public PictureFlowList()
        {
            InitializeComponent();
        }

        public void DisplayPictures(PictureCollection pictures)
        {
            this.PhotosListBox.ItemsSource = null;

            List<PictureCacheData> cacheList = new List<PictureCacheData>();
            foreach (PictureData picture in pictures)
            {
                PictureCacheData cacheData = PicContext.Current.PictureCache.GetPictureCacheData(
                    picture, PictureCacheSize.Small);
                if (cacheData.Filename != null)
                {
                    cacheList.Add(cacheData);
                }
            }

            this.PhotosListBox.ItemsSource = cacheList;
        }
    }

}
