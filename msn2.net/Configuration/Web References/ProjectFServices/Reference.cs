﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3705.209
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.0.3705.209.
// 
namespace msn2.net.Configuration.ProjectFServices {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DataServiceSoap", Namespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService")]
    public class DataService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public DataService() {
            string urlSetting = System.Configuration.ConfigurationSettings.AppSettings["msn2.net.Configuration.ProjectFServices.DataService"];
            if ((urlSetting != null)) {
                this.Url = string.Concat(urlSetting, "");
            }
            else {
                this.Url = "http://localhost/ProjectFServices/DataService.asmx";
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.msn2.net/ProjectFServices/2002/04/23/DataService/GetChildren", RequestNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", ResponseNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DataSetDataItem GetChildren(System.Guid id, System.Guid userId, string[] types) {
            object[] results = this.Invoke("GetChildren", new object[] {
                        id,
                        userId,
                        types});
            return ((DataSetDataItem)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetChildren(System.Guid id, System.Guid userId, string[] types, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetChildren", new object[] {
                        id,
                        userId,
                        types}, callback, asyncState);
        }
        
        /// <remarks/>
        public DataSetDataItem EndGetChildren(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((DataSetDataItem)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.msn2.net/ProjectFServices/2002/04/23/DataService/Get", RequestNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", ResponseNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DataSetDataItem Get(System.Guid id, System.Guid userId, string name, string url, string serializedData, string type, System.Guid itemKey) {
            object[] results = this.Invoke("Get", new object[] {
                        id,
                        userId,
                        name,
                        url,
                        serializedData,
                        type,
                        itemKey});
            return ((DataSetDataItem)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGet(System.Guid id, System.Guid userId, string name, string url, string serializedData, string type, System.Guid itemKey, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Get", new object[] {
                        id,
                        userId,
                        name,
                        url,
                        serializedData,
                        type,
                        itemKey}, callback, asyncState);
        }
        
        /// <remarks/>
        public DataSetDataItem EndGet(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((DataSetDataItem)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.msn2.net/ProjectFServices/2002/04/23/DataService/Save", RequestNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", ResponseNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Save(System.Guid id, string name, string serializedData, string type) {
            this.Invoke("Save", new object[] {
                        id,
                        name,
                        serializedData,
                        type});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSave(System.Guid id, string name, string serializedData, string type, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Save", new object[] {
                        id,
                        name,
                        serializedData,
                        type}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndSave(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.msn2.net/ProjectFServices/2002/04/23/DataService/Delete", RequestNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", ResponseNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Delete(System.Guid id) {
            this.Invoke("Delete", new object[] {
                        id});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginDelete(System.Guid id, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Delete", new object[] {
                        id}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndDelete(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://services.msn2.net/ProjectFServices/2002/04/23/DataService/Login", RequestNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", ResponseNamespace="http://services.msn2.net/ProjectFServices/2002/04/23/DataService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Login(string signinName, string machineName) {
            object[] results = this.Invoke("Login", new object[] {
                        signinName,
                        machineName});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLogin(string signinName, string machineName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Login", new object[] {
                        signinName,
                        machineName}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataSet EndLogin(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }
    }
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DataSetDataItem : DataSet {
        
        private DataItemDataTable tableDataItem;
        
        public DataSetDataItem() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DataSetDataItem(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["DataItem"] != null)) {
                    this.Tables.Add(new DataItemDataTable(ds.Tables["DataItem"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public DataItemDataTable DataItem {
            get {
                return this.tableDataItem;
            }
        }
        
        public override DataSet Clone() {
            DataSetDataItem cln = ((DataSetDataItem)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["DataItem"] != null)) {
                this.Tables.Add(new DataItemDataTable(ds.Tables["DataItem"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableDataItem = ((DataItemDataTable)(this.Tables["DataItem"]));
            if ((this.tableDataItem != null)) {
                this.tableDataItem.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DataSetDataItem";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DataSetDataItem.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableDataItem = new DataItemDataTable();
            this.Tables.Add(this.tableDataItem);
        }
        
        private bool ShouldSerializeDataItem() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void DataItemRowChangeEventHandler(object sender, DataItemRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DataItemDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnId;
            
            private DataColumn columnName;
            
            private DataColumn columnParentId;
            
            private DataColumn columnUserId;
            
            private DataColumn columnItemType;
            
            private DataColumn columnItemUrl;
            
            private DataColumn columnItemData;
            
            private DataColumn columnConfigTreeLocation;
            
            private DataColumn columnItemKey;
            
            private DataColumn columnHistoryItem;
            
            private DataColumn columnHistoryDate;
            
            private DataColumn columnTimestamp;
            
            internal DataItemDataTable() : 
                    base("DataItem") {
                this.InitClass();
            }
            
            internal DataItemDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IdColumn {
                get {
                    return this.columnId;
                }
            }
            
            internal DataColumn NameColumn {
                get {
                    return this.columnName;
                }
            }
            
            internal DataColumn ParentIdColumn {
                get {
                    return this.columnParentId;
                }
            }
            
            internal DataColumn UserIdColumn {
                get {
                    return this.columnUserId;
                }
            }
            
            internal DataColumn ItemTypeColumn {
                get {
                    return this.columnItemType;
                }
            }
            
            internal DataColumn ItemUrlColumn {
                get {
                    return this.columnItemUrl;
                }
            }
            
            internal DataColumn ItemDataColumn {
                get {
                    return this.columnItemData;
                }
            }
            
            internal DataColumn ConfigTreeLocationColumn {
                get {
                    return this.columnConfigTreeLocation;
                }
            }
            
            internal DataColumn ItemKeyColumn {
                get {
                    return this.columnItemKey;
                }
            }
            
            internal DataColumn HistoryItemColumn {
                get {
                    return this.columnHistoryItem;
                }
            }
            
            internal DataColumn HistoryDateColumn {
                get {
                    return this.columnHistoryDate;
                }
            }
            
            internal DataColumn TimestampColumn {
                get {
                    return this.columnTimestamp;
                }
            }
            
            public DataItemRow this[int index] {
                get {
                    return ((DataItemRow)(this.Rows[index]));
                }
            }
            
            public event DataItemRowChangeEventHandler DataItemRowChanged;
            
            public event DataItemRowChangeEventHandler DataItemRowChanging;
            
            public event DataItemRowChangeEventHandler DataItemRowDeleted;
            
            public event DataItemRowChangeEventHandler DataItemRowDeleting;
            
            public void AddDataItemRow(DataItemRow row) {
                this.Rows.Add(row);
            }
            
            public DataItemRow AddDataItemRow(System.Guid Id, string Name, System.Guid ParentId, System.Guid UserId, string ItemType, string ItemUrl, string ItemData, System.Guid ConfigTreeLocation, System.Guid ItemKey, bool HistoryItem, System.DateTime HistoryDate, System.DateTime Timestamp) {
                DataItemRow rowDataItemRow = ((DataItemRow)(this.NewRow()));
                rowDataItemRow.ItemArray = new object[] {
                        Id,
                        Name,
                        ParentId,
                        UserId,
                        ItemType,
                        ItemUrl,
                        ItemData,
                        ConfigTreeLocation,
                        ItemKey,
                        HistoryItem,
                        HistoryDate,
                        Timestamp};
                this.Rows.Add(rowDataItemRow);
                return rowDataItemRow;
            }
            
            public DataItemRow FindById(System.Guid Id) {
                return ((DataItemRow)(this.Rows.Find(new object[] {
                            Id})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                DataItemDataTable cln = ((DataItemDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new DataItemDataTable();
            }
            
            internal void InitVars() {
                this.columnId = this.Columns["Id"];
                this.columnName = this.Columns["Name"];
                this.columnParentId = this.Columns["ParentId"];
                this.columnUserId = this.Columns["UserId"];
                this.columnItemType = this.Columns["ItemType"];
                this.columnItemUrl = this.Columns["ItemUrl"];
                this.columnItemData = this.Columns["ItemData"];
                this.columnConfigTreeLocation = this.Columns["ConfigTreeLocation"];
                this.columnItemKey = this.Columns["ItemKey"];
                this.columnHistoryItem = this.Columns["HistoryItem"];
                this.columnHistoryDate = this.Columns["HistoryDate"];
                this.columnTimestamp = this.Columns["Timestamp"];
            }
            
            private void InitClass() {
                this.columnId = new DataColumn("Id", typeof(System.Guid), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnId);
                this.columnName = new DataColumn("Name", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnName);
                this.columnParentId = new DataColumn("ParentId", typeof(System.Guid), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnParentId);
                this.columnUserId = new DataColumn("UserId", typeof(System.Guid), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserId);
                this.columnItemType = new DataColumn("ItemType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnItemType);
                this.columnItemUrl = new DataColumn("ItemUrl", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnItemUrl);
                this.columnItemData = new DataColumn("ItemData", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnItemData);
                this.columnConfigTreeLocation = new DataColumn("ConfigTreeLocation", typeof(System.Guid), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnConfigTreeLocation);
                this.columnItemKey = new DataColumn("ItemKey", typeof(System.Guid), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnItemKey);
                this.columnHistoryItem = new DataColumn("HistoryItem", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHistoryItem);
                this.columnHistoryDate = new DataColumn("HistoryDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHistoryDate);
                this.columnTimestamp = new DataColumn("Timestamp", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTimestamp);
                this.Constraints.Add(new UniqueConstraint("DataSetDataItemKey1", new DataColumn[] {
                                this.columnId}, true));
                this.columnId.AllowDBNull = false;
                this.columnId.Unique = true;
            }
            
            public DataItemRow NewDataItemRow() {
                return ((DataItemRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new DataItemRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(DataItemRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.DataItemRowChanged != null)) {
                    this.DataItemRowChanged(this, new DataItemRowChangeEvent(((DataItemRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.DataItemRowChanging != null)) {
                    this.DataItemRowChanging(this, new DataItemRowChangeEvent(((DataItemRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.DataItemRowDeleted != null)) {
                    this.DataItemRowDeleted(this, new DataItemRowChangeEvent(((DataItemRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.DataItemRowDeleting != null)) {
                    this.DataItemRowDeleting(this, new DataItemRowChangeEvent(((DataItemRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveDataItemRow(DataItemRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DataItemRow : DataRow {
            
            private DataItemDataTable tableDataItem;
            
            internal DataItemRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableDataItem = ((DataItemDataTable)(this.Table));
            }
            
            public System.Guid Id {
                get {
                    return ((System.Guid)(this[this.tableDataItem.IdColumn]));
                }
                set {
                    this[this.tableDataItem.IdColumn] = value;
                }
            }
            
            public string Name {
                get {
                    try {
                        return ((string)(this[this.tableDataItem.NameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.NameColumn] = value;
                }
            }
            
            public System.Guid ParentId {
                get {
                    try {
                        return ((System.Guid)(this[this.tableDataItem.ParentIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ParentIdColumn] = value;
                }
            }
            
            public System.Guid UserId {
                get {
                    try {
                        return ((System.Guid)(this[this.tableDataItem.UserIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.UserIdColumn] = value;
                }
            }
            
            public string ItemType {
                get {
                    try {
                        return ((string)(this[this.tableDataItem.ItemTypeColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ItemTypeColumn] = value;
                }
            }
            
            public string ItemUrl {
                get {
                    try {
                        return ((string)(this[this.tableDataItem.ItemUrlColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ItemUrlColumn] = value;
                }
            }
            
            public string ItemData {
                get {
                    try {
                        return ((string)(this[this.tableDataItem.ItemDataColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ItemDataColumn] = value;
                }
            }
            
            public System.Guid ConfigTreeLocation {
                get {
                    try {
                        return ((System.Guid)(this[this.tableDataItem.ConfigTreeLocationColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ConfigTreeLocationColumn] = value;
                }
            }
            
            public System.Guid ItemKey {
                get {
                    try {
                        return ((System.Guid)(this[this.tableDataItem.ItemKeyColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.ItemKeyColumn] = value;
                }
            }
            
            public bool HistoryItem {
                get {
                    try {
                        return ((bool)(this[this.tableDataItem.HistoryItemColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.HistoryItemColumn] = value;
                }
            }
            
            public System.DateTime HistoryDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableDataItem.HistoryDateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.HistoryDateColumn] = value;
                }
            }
            
            public System.DateTime Timestamp {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableDataItem.TimestampColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableDataItem.TimestampColumn] = value;
                }
            }
            
            public bool IsNameNull() {
                return this.IsNull(this.tableDataItem.NameColumn);
            }
            
            public void SetNameNull() {
                this[this.tableDataItem.NameColumn] = System.Convert.DBNull;
            }
            
            public bool IsParentIdNull() {
                return this.IsNull(this.tableDataItem.ParentIdColumn);
            }
            
            public void SetParentIdNull() {
                this[this.tableDataItem.ParentIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsUserIdNull() {
                return this.IsNull(this.tableDataItem.UserIdColumn);
            }
            
            public void SetUserIdNull() {
                this[this.tableDataItem.UserIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsItemTypeNull() {
                return this.IsNull(this.tableDataItem.ItemTypeColumn);
            }
            
            public void SetItemTypeNull() {
                this[this.tableDataItem.ItemTypeColumn] = System.Convert.DBNull;
            }
            
            public bool IsItemUrlNull() {
                return this.IsNull(this.tableDataItem.ItemUrlColumn);
            }
            
            public void SetItemUrlNull() {
                this[this.tableDataItem.ItemUrlColumn] = System.Convert.DBNull;
            }
            
            public bool IsItemDataNull() {
                return this.IsNull(this.tableDataItem.ItemDataColumn);
            }
            
            public void SetItemDataNull() {
                this[this.tableDataItem.ItemDataColumn] = System.Convert.DBNull;
            }
            
            public bool IsConfigTreeLocationNull() {
                return this.IsNull(this.tableDataItem.ConfigTreeLocationColumn);
            }
            
            public void SetConfigTreeLocationNull() {
                this[this.tableDataItem.ConfigTreeLocationColumn] = System.Convert.DBNull;
            }
            
            public bool IsItemKeyNull() {
                return this.IsNull(this.tableDataItem.ItemKeyColumn);
            }
            
            public void SetItemKeyNull() {
                this[this.tableDataItem.ItemKeyColumn] = System.Convert.DBNull;
            }
            
            public bool IsHistoryItemNull() {
                return this.IsNull(this.tableDataItem.HistoryItemColumn);
            }
            
            public void SetHistoryItemNull() {
                this[this.tableDataItem.HistoryItemColumn] = System.Convert.DBNull;
            }
            
            public bool IsHistoryDateNull() {
                return this.IsNull(this.tableDataItem.HistoryDateColumn);
            }
            
            public void SetHistoryDateNull() {
                this[this.tableDataItem.HistoryDateColumn] = System.Convert.DBNull;
            }
            
            public bool IsTimestampNull() {
                return this.IsNull(this.tableDataItem.TimestampColumn);
            }
            
            public void SetTimestampNull() {
                this[this.tableDataItem.TimestampColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class DataItemRowChangeEvent : EventArgs {
            
            private DataItemRow eventRow;
            
            private DataRowAction eventAction;
            
            public DataItemRowChangeEvent(DataItemRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public DataItemRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
