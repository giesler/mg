using System;
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
using Microsoft.Phone.Shell;

namespace giesler.org.lists
{
    public partial class EditStorePage : PhoneApplicationPage
    {
        public EditStorePage()
        {
            InitializeComponent();

            App.Current.LoadAll();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
/*
            if (App.LiveContacts == null)
            {
            }
            else
            {
                foreach (Contact c in App.LiveContacts.OrderBy(i => i.FormattedName))
                {
                    this.list.Items.Add(c.FormattedName);
                }
            }
  */
            this.list.Items.Add("not yet implemented");
            this.owners.Items.Add("not yet implemented");

            Guid listUnquieId = new Guid(NavigationContext.QueryString["listUniqueId"]);
            List list = App.Lists.First(i => i.UniqueId == listUnquieId);

            this.pivot.Title = list.Name.ToLower();            
        }

        void OnDelete(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete this list?  You cannot undo this delete.";
            if (MessageBox.Show(message, "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.pbar.Visibility = System.Windows.Visibility.Visible;
                this.status.Text = "deleting...";
                this.status.Visibility = System.Windows.Visibility.Visible;

                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;

                Guid listUnquieId = new Guid(NavigationContext.QueryString["listUniqueId"]);

                ListDataServiceClient svc = App.DataProvider;
                svc.DeleteListCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(svc_DeleteListCompleted);
                svc.DeleteListAsync(App.AuthDataList, listUnquieId);
            }
        }

        void svc_DeleteListCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);

                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = true;
                this.pbar.Visibility = System.Windows.Visibility.Collapsed;
                this.status.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Guid listUnqiueId = new Guid(NavigationContext.QueryString["listUniqueId"]);
                ListEx list = App.Lists.First(l => l.UniqueId == listUnqiueId);
                App.Lists.Remove(list);
                App.Current.SaveAll();

                NavigationService.GoBack();
            }

            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }
    }
}