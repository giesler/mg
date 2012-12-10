﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Live;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public partial class EditStorePage : PhoneApplicationPage
    {
        public EditStorePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.LiveContacts == null)
            {
                this.list.Items.Add("contacts not loaded");
            }
            else
            {
                foreach (Contact c in App.LiveContacts.OrderBy(i => i.FormattedName))
                {
                    this.list.Items.Add(c.FormattedName);
                }
            }
            
            Guid listUnqiueId = new Guid(NavigationContext.QueryString["listUniqueId"]);
            List list = App.Lists.First(i => i.UniqueId == listUnqiueId);

            this.pivot.Title = list.Name;
        }
    }
}