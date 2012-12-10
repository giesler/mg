﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.CompactFramework.Design.Data, Version 2.0.50727.1433.
// 
namespace mn2.net.ShoppingList.sls {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Binding1_IShoppingListService", Namespace="http://tempuri.org/")]
    public partial class ShoppingListService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public ShoppingListService() {
            this.Url = "http://svc.msn2.net/sl/sls.svc";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/GetStores", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] GetStores() {
            object[] results = this.Invoke("GetStores", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetStores(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetStores", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string[] EndGetStores(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/GetShoppingListItems", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.datacontract.org/2004/07/msn2.net.ShoppingList")]
        public ShoppingListItem[] GetShoppingListItems() {
            object[] results = this.Invoke("GetShoppingListItems", new object[0]);
            return ((ShoppingListItem[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetShoppingListItems(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetShoppingListItems", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public ShoppingListItem[] EndGetShoppingListItems(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShoppingListItem[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/GetShoppingListItemsForStore", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.datacontract.org/2004/07/msn2.net.ShoppingList")]
        public ShoppingListItem[] GetShoppingListItemsForStore([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string store) {
            object[] results = this.Invoke("GetShoppingListItemsForStore", new object[] {
                        store});
            return ((ShoppingListItem[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetShoppingListItemsForStore(string store, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetShoppingListItemsForStore", new object[] {
                        store}, callback, asyncState);
        }
        
        /// <remarks/>
        public ShoppingListItem[] EndGetShoppingListItemsForStore(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShoppingListItem[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/AddShoppingListItem", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public ShoppingListItem AddShoppingListItem([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string store, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string listItem) {
            object[] results = this.Invoke("AddShoppingListItem", new object[] {
                        store,
                        listItem});
            return ((ShoppingListItem)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginAddShoppingListItem(string store, string listItem, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("AddShoppingListItem", new object[] {
                        store,
                        listItem}, callback, asyncState);
        }
        
        /// <remarks/>
        public ShoppingListItem EndAddShoppingListItem(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShoppingListItem)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/DeleteShoppingListItem", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteShoppingListItem([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ShoppingListItem listItem) {
            this.Invoke("DeleteShoppingListItem", new object[] {
                        listItem});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginDeleteShoppingListItem(ShoppingListItem listItem, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("DeleteShoppingListItem", new object[] {
                        listItem}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndDeleteShoppingListItem(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IShoppingListService/UpdateShoppingListItem", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void UpdateShoppingListItem([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ShoppingListItem listItem) {
            this.Invoke("UpdateShoppingListItem", new object[] {
                        listItem});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginUpdateShoppingListItem(ShoppingListItem listItem, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("UpdateShoppingListItem", new object[] {
                        listItem}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndUpdateShoppingListItem(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/msn2.net.ShoppingList")]
    public partial class ShoppingListItem {
        
        private int idField;
        
        private bool idFieldSpecified;
        
        private string listItemField;
        
        private string storeField;
        
        /// <remarks/>
        public int Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ListItem {
            get {
                return this.listItemField;
            }
            set {
                this.listItemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Store {
            get {
                return this.storeField;
            }
            set {
                this.storeField = value;
            }
        }
    }
}
