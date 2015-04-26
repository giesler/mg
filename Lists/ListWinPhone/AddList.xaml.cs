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
using giesler.org.lists.ListData;
using Microsoft.Phone.Shell;

namespace giesler.org.lists
{
    public partial class AddList : PhoneApplicationPage
    {
        public AddList()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.name.Focus();
        }

        private void OnAdd(object sender, EventArgs e)
        {
            this.ToggleControls(false);

            ListDataServiceClient svc = App.DataProvider;
            svc.AddListCompleted += new EventHandler<AddListCompletedEventArgs>(svc_AddListCompleted);
            svc.AddListAsync(App.AuthDataList, this.name.Text.Trim());
        }

        void svc_AddListCompleted(object sender, AddListCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                this.ToggleControls(true);
            }
            else if (e.Result.IsDuplicate)
            {
                string message = string.Format("The list '{0}' already exists.", this.name.Text);
                MessageBox.Show(message, "Duplicate", MessageBoxButton.OK);
                this.ToggleControls(true);
            }
            else
            {
                ListEx newList = new ListEx();
                newList.UniqueId = e.Result.List.UniqueId;
                newList.Name = e.Result.List.Name;
                newList.ListItems = new ListItem[0];
                App.Lists.Add(newList);
                App.Current.SaveAll();

                NavigationService.GoBack();
            }

            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        void ToggleControls(bool enableControls)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = enableControls;
            this.pbar.Visibility = enableControls ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            this.status.Visibility = enableControls ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            this.name.IsEnabled = enableControls;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.name.Text == @"<name>")
            {
                this.name.Text = string.Empty;
            }
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = this.name.Text.Trim().Length > 1;
        }
    }
}