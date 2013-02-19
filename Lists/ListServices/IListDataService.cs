using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace msn2.net.ShoppingList
{
    [ServiceContract(Namespace="http://svc.listgo.mobi")]
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
        List<GetAllResult> GetAll(ClientAuthenticationData auth);

        [OperationContract]
        List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId);

        [OperationContract]
        List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth);

        [OperationContract]
        AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name);

        [OperationContract]
        AddListItemReturnValue AddListItemWithId(ClientAuthenticationData auth, Guid listUniqueId, Guid itemUniqueId, string name);

        [OperationContract]
        AddListItemsReturnValue AddListItems(ClientAuthenticationData auth, Guid listUniqueId, ListItem[] items);

        [OperationContract]
        UpdateListItemReturnData UpdateListItem(ClientAuthenticationData auth, ListItem item);

        [OperationContract]
        DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId);

        [OperationContract]
        DateTime GetLastChangeTime(ClientAuthenticationData auth);

        [OperationContract]
        Person GetPerson(string liveUserId, string name);

        [OperationContract]
        void UpdatePerson(ClientAuthenticationData auth, Person person);

        [OperationContract]
        PersonDevice AddDevice(ClientAuthenticationData auth, string deviceName);

        [OperationContract]
        void RemoveDevice(ClientAuthenticationData auth, Guid deviceId);

        [OperationContract]
        List<string> GetCommonItems(ClientAuthenticationData auth, Guid listUniqueId);
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

    [DataContract]
    public class AddListItemsReturnValue
    {
        public AddListItemsReturnValue() { }

        [DataMember]
        public ListItem[] Items { get; set; }

        [DataMember]
        public bool[] IsDuplicate { get; set; }
    }
}
