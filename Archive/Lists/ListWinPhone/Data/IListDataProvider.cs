using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    interface IListDataProvider
    {
        void AddListItemAsync(giesler.org.lists.ListData.ClientAuthenticationData auth, System.Guid listUniqueId, string name);
        void AddListAsync(giesler.org.lists.ListData.ClientAuthenticationData auth, string name);
        void GetListsAsync(giesler.org.lists.ListData.ClientAuthenticationData auth);
        void GetAllListItemsAsync(giesler.org.lists.ListData.ClientAuthenticationData auth);
        void DeleteListItemAsync(giesler.org.lists.ListData.ClientAuthenticationData auth, System.Guid listItemUniqueId);                
        void CloseAsync();
        
        event System.EventHandler<AddListCompletedEventArgs> AddListCompleted;
        event System.EventHandler<UpdateListCompletedEventArgs> UpdateListCompleted;
        event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DeleteListCompleted;
        event System.EventHandler<GetListsCompletedEventArgs> GetListsCompleted;
        event System.EventHandler<GetListItemsCompletedEventArgs> GetListItemsCompleted;
        event System.EventHandler<GetAllListItemsCompletedEventArgs> GetAllListItemsCompleted;
        event System.EventHandler<AddListItemCompletedEventArgs> AddListItemCompleted;
        event System.EventHandler<UpdateListItemCompletedEventArgs> UpdateListItemCompleted;
        event System.EventHandler<DeleteListItemCompletedEventArgs> DeleteListItemCompleted;
        event System.EventHandler<GetLastChangeTimeCompletedEventArgs> GetLastChangeTimeCompleted;
        event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
    }
}
