﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace msn2.net.ShoppingList
{
    [ServiceContract]
    public interface IListDataService
    {
        [OperationContract]
        AddListReturnValue AddList(ClientAuthenticationData auth, string name);

        [OperationContract]
        UpdateListReturnValue UpdateList(ClientAuthenticationData auth, List list);

        [OperationContract]
        void DeleteList(ClientAuthenticationData auth, Guid uniqueId);

        [OperationContract]
        List<GetListsResult> GetLists(ClientAuthenticationData auth);

        [OperationContract]
        List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId);

        [OperationContract]
        List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth);

        [OperationContract]
        AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name);

        [OperationContract]
        UpdateListItemReturnData UpdateListItem(ClientAuthenticationData auth, ListItem item);

        [OperationContract]
        DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId);

        [OperationContract]
        DateTime GetLastChangeTime(ClientAuthenticationData auth);
    }

    [DataContract]
    public class AddListReturnValue
    {
        public AddListReturnValue() { }

        [DataMember]
        public List List { get; set; }

        [DataMember]
        public bool IsDuplicate { get; set; }
    }

    [DataContract]
    public class AddListItemReturnValue
    {
        public AddListItemReturnValue() { }

        [DataMember]
        public ListItem ListItem { get; set; }

        [DataMember]
        public bool IsDuplicate { get; set; }
    }

    [DataContract]
    public class UpdateListReturnValue
    {
        public UpdateListReturnValue() { }

        [DataMember]
        public bool IsDuplicate { get; set; }

        [DataMember]
        public bool IsInvalidId { get; set; }
    }
}
