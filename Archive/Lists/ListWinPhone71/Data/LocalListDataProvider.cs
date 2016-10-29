using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace giesler.org.lists.Data
{
    public class LocalListDataProvider: IListDataProvider
    {
        #region IListDataProvider Members

        public void AddListItemAsync(ListData.ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            throw new NotImplementedException();
        }

        public void AddListAsync(ListData.ClientAuthenticationData auth, string name)
        {
            throw new NotImplementedException();
        }

        public void GetListsAsync(ListData.ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        public void GetAllListItemsAsync(ListData.ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        public void DeleteListItemAsync(ListData.ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            throw new NotImplementedException();
        }

        public void CloseAsync()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ListData.AddListCompletedEventArgs> AddListCompleted;

        public event EventHandler<ListData.UpdateListCompletedEventArgs> UpdateListCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DeleteListCompleted;

        public event EventHandler<ListData.GetListsCompletedEventArgs> GetListsCompleted;

        public event EventHandler<ListData.GetListItemsCompletedEventArgs> GetListItemsCompleted;

        public event EventHandler<ListData.GetAllListItemsCompletedEventArgs> GetAllListItemsCompleted;

        public event EventHandler<ListData.AddListItemCompletedEventArgs> AddListItemCompleted;

        public event EventHandler<ListData.UpdateListItemCompletedEventArgs> UpdateListItemCompleted;

        public event EventHandler<ListData.DeleteListItemCompletedEventArgs> DeleteListItemCompleted;

        public event EventHandler<ListData.GetLastChangeTimeCompletedEventArgs> GetLastChangeTimeCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;

        #endregion
    }
}
