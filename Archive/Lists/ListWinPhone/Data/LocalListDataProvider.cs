using giesler.org.lists.ListData;
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

        public void AddListItemAsync(ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            throw new NotImplementedException();
        }

        public void AddListAsync(ClientAuthenticationData auth, string name)
        {
            throw new NotImplementedException();
        }

        public void GetListsAsync(ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        public void GetAllListItemsAsync(ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        public void DeleteListItemAsync(ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            throw new NotImplementedException();
        }

        public void CloseAsync()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<AddListCompletedEventArgs> AddListCompleted;

        public event EventHandler<UpdateListCompletedEventArgs> UpdateListCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DeleteListCompleted;

        public event EventHandler<GetListsCompletedEventArgs> GetListsCompleted;

        public event EventHandler<GetListItemsCompletedEventArgs> GetListItemsCompleted;

        public event EventHandler<GetAllListItemsCompletedEventArgs> GetAllListItemsCompleted;

        public event EventHandler<AddListItemCompletedEventArgs> AddListItemCompleted;

        public event EventHandler<UpdateListItemCompletedEventArgs> UpdateListItemCompleted;

        public event EventHandler<DeleteListItemCompletedEventArgs> DeleteListItemCompleted;

        public event EventHandler<GetLastChangeTimeCompletedEventArgs> GetLastChangeTimeCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;

        #endregion
    }
}
