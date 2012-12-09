﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3328.4
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace PicAdminCS {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DataSetCategoryGroup : DataSet {
        
        private CategoryGroupDataTable tableCategoryGroup;
        
        public DataSetCategoryGroup() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DataSetCategoryGroup(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["CategoryGroup"] != null)) {
                    this.Tables.Add(new CategoryGroupDataTable(ds.Tables["CategoryGroup"]));
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
        public CategoryGroupDataTable CategoryGroup {
            get {
                return this.tableCategoryGroup;
            }
        }
        
        public override DataSet Clone() {
            DataSetCategoryGroup cln = ((DataSetCategoryGroup)(base.Clone()));
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
            if ((ds.Tables["CategoryGroup"] != null)) {
                this.Tables.Add(new CategoryGroupDataTable(ds.Tables["CategoryGroup"]));
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
            this.tableCategoryGroup = ((CategoryGroupDataTable)(this.Tables["CategoryGroup"]));
            if ((this.tableCategoryGroup != null)) {
                this.tableCategoryGroup.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DataSetCategoryGroup";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DataSetCategoryGroup.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableCategoryGroup = new CategoryGroupDataTable();
            this.Tables.Add(this.tableCategoryGroup);
        }
        
        private bool ShouldSerializeCategoryGroup() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void CategoryGroupRowChangeEventHandler(object sender, CategoryGroupRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryGroupDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnCategoryGroupID;
            
            private DataColumn columnCategoryID;
            
            private DataColumn columnGroupID;
            
            internal CategoryGroupDataTable() : 
                    base("CategoryGroup") {
                this.InitClass();
            }
            
            internal CategoryGroupDataTable(DataTable table) : 
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
            
            internal DataColumn CategoryGroupIDColumn {
                get {
                    return this.columnCategoryGroupID;
                }
            }
            
            internal DataColumn CategoryIDColumn {
                get {
                    return this.columnCategoryID;
                }
            }
            
            internal DataColumn GroupIDColumn {
                get {
                    return this.columnGroupID;
                }
            }
            
            public CategoryGroupRow this[int index] {
                get {
                    return ((CategoryGroupRow)(this.Rows[index]));
                }
            }
            
            public event CategoryGroupRowChangeEventHandler CategoryGroupRowChanged;
            
            public event CategoryGroupRowChangeEventHandler CategoryGroupRowChanging;
            
            public event CategoryGroupRowChangeEventHandler CategoryGroupRowDeleted;
            
            public event CategoryGroupRowChangeEventHandler CategoryGroupRowDeleting;
            
            public void AddCategoryGroupRow(CategoryGroupRow row) {
                this.Rows.Add(row);
            }
            
            public CategoryGroupRow AddCategoryGroupRow(int CategoryID, int GroupID) {
                CategoryGroupRow rowCategoryGroupRow = ((CategoryGroupRow)(this.NewRow()));
                rowCategoryGroupRow.ItemArray = new object[] {
                        null,
                        CategoryID,
                        GroupID};
                this.Rows.Add(rowCategoryGroupRow);
                return rowCategoryGroupRow;
            }
            
            public CategoryGroupRow FindByCategoryGroupID(int CategoryGroupID) {
                return ((CategoryGroupRow)(this.Rows.Find(new object[] {
                            CategoryGroupID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CategoryGroupDataTable cln = ((CategoryGroupDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            internal void InitVars() {
                this.columnCategoryGroupID = this.Columns["CategoryGroupID"];
                this.columnCategoryID = this.Columns["CategoryID"];
                this.columnGroupID = this.Columns["GroupID"];
            }
            
            private void InitClass() {
                this.columnCategoryGroupID = new DataColumn("CategoryGroupID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryGroupID);
                this.columnCategoryID = new DataColumn("CategoryID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryID);
                this.columnGroupID = new DataColumn("GroupID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnGroupID);
                this.Constraints.Add(new UniqueConstraint("DataSetCategoryGroupKey1", new DataColumn[] {
                                this.columnCategoryGroupID}, true));
                this.columnCategoryGroupID.AutoIncrement = true;
                this.columnCategoryGroupID.AllowDBNull = false;
                this.columnCategoryGroupID.ReadOnly = true;
                this.columnCategoryGroupID.Unique = true;
            }
            
            public CategoryGroupRow NewCategoryGroupRow() {
                return ((CategoryGroupRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CategoryGroupRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CategoryGroupRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CategoryGroupRowChanged != null)) {
                    this.CategoryGroupRowChanged(this, new CategoryGroupRowChangeEvent(((CategoryGroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CategoryGroupRowChanging != null)) {
                    this.CategoryGroupRowChanging(this, new CategoryGroupRowChangeEvent(((CategoryGroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CategoryGroupRowDeleted != null)) {
                    this.CategoryGroupRowDeleted(this, new CategoryGroupRowChangeEvent(((CategoryGroupRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CategoryGroupRowDeleting != null)) {
                    this.CategoryGroupRowDeleting(this, new CategoryGroupRowChangeEvent(((CategoryGroupRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCategoryGroupRow(CategoryGroupRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryGroupRow : DataRow {
            
            private CategoryGroupDataTable tableCategoryGroup;
            
            internal CategoryGroupRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCategoryGroup = ((CategoryGroupDataTable)(this.Table));
            }
            
            public int CategoryGroupID {
                get {
                    return ((int)(this[this.tableCategoryGroup.CategoryGroupIDColumn]));
                }
                set {
                    this[this.tableCategoryGroup.CategoryGroupIDColumn] = value;
                }
            }
            
            public int CategoryID {
                get {
                    try {
                        return ((int)(this[this.tableCategoryGroup.CategoryIDColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCategoryGroup.CategoryIDColumn] = value;
                }
            }
            
            public int GroupID {
                get {
                    try {
                        return ((int)(this[this.tableCategoryGroup.GroupIDColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCategoryGroup.GroupIDColumn] = value;
                }
            }
            
            public bool IsCategoryIDNull() {
                return this.IsNull(this.tableCategoryGroup.CategoryIDColumn);
            }
            
            public void SetCategoryIDNull() {
                this[this.tableCategoryGroup.CategoryIDColumn] = System.Convert.DBNull;
            }
            
            public bool IsGroupIDNull() {
                return this.IsNull(this.tableCategoryGroup.GroupIDColumn);
            }
            
            public void SetGroupIDNull() {
                this[this.tableCategoryGroup.GroupIDColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryGroupRowChangeEvent : EventArgs {
            
            private CategoryGroupRow eventRow;
            
            private DataRowAction eventAction;
            
            public CategoryGroupRowChangeEvent(CategoryGroupRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CategoryGroupRow Row {
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