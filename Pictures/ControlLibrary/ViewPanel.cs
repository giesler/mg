﻿#region Using directives

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
    public enum ViewMode
    {
        Category = 0,
        DatePictureTaken,
        DatePictureAdded,
        Person
    }

    public partial class ViewPanel : UserControl
    {
        private ViewMode viewMode = ViewMode.Category;

        private CategoryTree categoryTree;
        private PeopleCtl peoplePicker;
        private DateSelector addedDateSelector;
        private DateSelector takenDateSelector;

        public ViewPanel()
        {
            InitializeComponent();

            if (!this.DesignMode)
            {
                LoadView();
            }
        }

        public void SetView(ViewMode viewMode)
        {
            this.viewMode = viewMode;
            this.LoadView();
        }

        public void RefreshData()
        {
            if (this.DesignMode)
            {
                return;
            }

            this.Controls.Clear();

            categoryTree = null;
            peoplePicker = null;
            addedDateSelector = null;
            takenDateSelector = null;

            LoadView();
        }

        private void LoadView()
        {
            if (this.DesignMode)
            {
                return;
            }

            switch (this.viewMode)
            {
                case ViewMode.Category:
                    ShowCategory();
                    break;
                case ViewMode.DatePictureTaken:
                    ShowDatePictureTaken();
                    break;
                case ViewMode.DatePictureAdded:
                    ShowDatePictureAdded();
                    break;
                case ViewMode.Person:
                    ShowPerson();
                    break;
            }
        }

        private void ShowDatePictureTaken()
        {
            if (null == takenDateSelector)
            {
                takenDateSelector = new DateSelector(PicContext.Current.PictureManager.GetPictureDates(), "PictureDate");
                takenDateSelector.ItemSelected += new EventHandler(takenDateSelector_ItemSelected);
                takenDateSelector.Dock = DockStyle.Fill;
            }

            this.Controls.Clear();
            this.Controls.Add(takenDateSelector);
        }

        private void ShowDatePictureAdded()
        {
            if (null == addedDateSelector)
            {
                addedDateSelector = new DateSelector(PicContext.Current.PictureManager.GetPictureAddedDates(), "PictureAddDate");
                addedDateSelector.ItemSelected += new EventHandler(addedDateSelector_ItemSelected);
                addedDateSelector.Dock = DockStyle.Fill;
            }

            this.Controls.Clear();
            this.Controls.Add(addedDateSelector);
        }

        private void ShowCategory()
        {
            if (null == categoryTree)
            {
                categoryTree = new CategoryTree();
                categoryTree.Dock = DockStyle.Fill;
                categoryTree.ClickCategory += new ClickCategoryEventHandler(categoryTree_ClickCategory);
            }

            this.Controls.Clear();
            this.Controls.Add(categoryTree);
        }

        private void ShowPerson()
        {
            if (null == peoplePicker)
            {
                peoplePicker = new PeopleCtl();
                peoplePicker.Dock = DockStyle.Fill;
            }

            this.Controls.Clear();
            this.Controls.Add(peoplePicker);
        }



        void addedDateSelector_ItemSelected(object sender, EventArgs e)
        {
            whereClause = addedDateSelector.WhereClause;

            if (null != RefreshView)
            {
                RefreshView(this, EventArgs.Empty);
            }
        }

        void takenDateSelector_ItemSelected(object sender, EventArgs e)
        {
            whereClause = takenDateSelector.WhereClause;

            if (null != RefreshView)
            {
                RefreshView(this, EventArgs.Empty);
            }
        }

        private string whereClause;

        public string WhereClause
        {
            get
            {
                return whereClause;
            }
        }

        public event EventHandler RefreshView;

        void categoryTree_ClickCategory(object sender, CategoryTreeEventArgs e)
        {
            whereClause = string.Format("p.PictureID in (select PictureID from PictureCategory where CategoryID IN (select SubCategoryId from CategorySubCategory where CategoryId = {0}))", e.Category.Id);

            if (null != RefreshView)
            {
                RefreshView(this, EventArgs.Empty);
            }
        }

    }
}
