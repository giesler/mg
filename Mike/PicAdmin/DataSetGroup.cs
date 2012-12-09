﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.2914.16
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace PicAdmin {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class DataSetGroup : System.Data.DataSet {
        
        private GroupDataTable tableGroup;
        
        public DataSetGroup() {
            this.InitClass();
        }
        
        private DataSetGroup(SerializationInfo info, StreamingContext context) {
            this.InitClass();
            this.GetSerializationData(info, context);
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public GroupDataTable Group {
            get {
                return this.tableGroup;
            }
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.ReadXml(reader, XmlReadMode.IgnoreSchema);
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        private void InitClass() {
            this.DataSetName = "DataSetGroup";
            this.Namespace = "http://www.tempuri.org/DataSetGroup.xsd";
            this.tableGroup = new GroupDataTable();
            this.Tables.Add(this.tableGroup);
        }
        
        private bool ShouldSerializeGroup() {
            return false;
        }
        
        public delegate void GroupRowChangeEventHandler(object sender, GroupRowChangeEvent e);
        
        public class GroupDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnGroupID;
            
            private DataColumn columnGroupName;
            
            internal GroupDataTable() : 
                    base("Group") {
                this.InitClass();
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn GroupIDColumn {
                get {
                    return this.columnGroupID;
                }
            }
            
            internal DataColumn GroupNameColumn {
                get {
                    return this.columnGroupName;
                }
            }
            
            public GroupRow this[int index] {
                get {
                    return ((GroupRow)(this.Rows[index]));
                }
            }
            
            public event GroupRowChangeEventHandler GroupRowChanged;
            
            public event GroupRowChangeEventHandler GroupRowChanging;
            
            public event GroupRowChangeEventHandler GroupRowDeleted;
            
            public event GroupRowChangeEventHandler GroupRowDeleting;
            
            public void AddGroupRow(GroupRow row) {
                this.Rows.Add(row);
            }
            
            public GroupRow AddGroupRow(string GroupName) {
                GroupRow rowGroupRow = ((GroupRow)(this.NewRow()));
                rowGroupRow.ItemArray = new object[] {
                        null,
                        GroupName};
                this.Rows.Add(rowGroupRow);
                return rowGroupRow;
            }
            
            public GroupRow FindByGroupID(int GroupID) {
                return ((GroupRow)(this.Rows.Find(new object[] {
                            GroupID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            private void InitClass() {
                this.columnGroupID = new DataColumn("GroupID", typeof(int), "", System.Data.MappingType.Element);
                this.columnGroupID.AutoIncrement = true;
                this.columnGroupID.AllowDBNull = false;
                this.columnGroupID.ReadOnly = true;
                this.columnGroupID.Unique = true;
                this.Columns.Add(this.columnGroupID);
                this.columnGroupName = new DataColumn("GroupName", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnGroupName);
                this.PrimaryKey = new DataColumn[] {
                        this.columnGroupID};
            }
            
            public GroupRow NewGroupRow() {
                return ((GroupRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                // We need to ensure that all Rows in the tabled are typed rows.
                // Table calls newRow whenever it needs to create a row.
                // So the following conditions are covered by Row newRow(Record record)
                // * Cursor calls table.addRecord(record) 
                // * table.addRow(object[] values) calls newRow(record)    
                return new GroupRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(GroupRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.GroupRowChanged != null)) {
                    this.GroupRowChanged(this, new GroupRowChangeEvent(((GroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.GroupRowChanging != null)) {
                    this.GroupRowChanging(this, new GroupRowChangeEvent(((GroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.GroupRowDeleted != null)) {
                    this.GroupRowDeleted(this, new GroupRowChangeEvent(((GroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.GroupRowDeleting != null)) {
                    this.GroupRowDeleting(this, new GroupRowChangeEvent(((GroupRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveGroupRow(GroupRow row) {
                this.Rows.Remove(row);
            }
        }
        
        public class GroupRow : DataRow {
            
            private GroupDataTable tableGroup;
            
            internal GroupRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableGroup = ((GroupDataTable)(this.Table));
            }
            
            public int GroupID {
                get {
                    return ((int)(this[this.tableGroup.GroupIDColumn]));
                }
                set {
                    this[this.tableGroup.GroupIDColumn] = value;
                }
            }
            
            public string GroupName {
                get {
                    try {
                        return ((string)(this[this.tableGroup.GroupNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGroup.GroupNameColumn] = value;
                }
            }
            
            public bool IsGroupNameNull() {
                return this.IsNull(this.tableGroup.GroupNameColumn);
            }
            
            public void SetGroupNameNull() {
                this[this.tableGroup.GroupNameColumn] = System.Convert.DBNull;
            }
        }
        
        public class GroupRowChangeEvent : EventArgs {
            
            private GroupRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            public GroupRowChangeEvent(GroupRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public GroupRow Row {
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
