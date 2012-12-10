using System;
using System.Collections.Generic;

namespace msn2.net.Pictures.Controls
{
    interface IPictureList
    {
        Picture GetNextPicture(int pictureId);
        Picture GetPreviousPicture(int pictureId);
        Picture GetSelectedPictureData();

        int ItemCount { get; }
        List<int> SelectedItems { get; set; }
        
        event PictureItemEventHandler ItemSelected;
        event PictureItemEventHandler ItemUnselected;
        event EventHandler MultiSelectEnd;
        event EventHandler MultiSelectStart;
        event PictureItemEventHandler PictureDoubleClick;
        event PictureItemEventHandler SelectedChanged;

        void LoadPictures(System.Collections.Generic.List<Picture> pictures);
        void ReleasePicture(int pictureId);
        void ReloadPicture(int pictureId);
        void Remove(int pictureId);
        void SelectAll();
        void ClearAll();
        void SetImageSize(int squareSize);
    }
}
