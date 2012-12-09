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
    public class DataSetCategory : DataSet {
        
        private CategoryDataTable tableCategory;
        
        private CategoryGroupDataTable tableCategoryGroup;
        
        private DataRelation relationCategoryCategoryGroup;
        
        public DataSetCategory() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DataSetCategory(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Category"] != null)) {
                    this.Tables.Add(new CategoryDataTable(ds.Tables["Category"]));
                }
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
        public CategoryDataTable Category {
            get {
                return this.tableCategory;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public CategoryGroupDataTable CategoryGroup {
            get {
                return this.tableCategoryGroup;
            }
        }
        
        public override DataSet Clone() {
            DataSetCategory cln = ((DataSetCategory)(base.Clone()));
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
            if ((ds.Tables["Category"] != null)) {
                this.Tables.Add(new CategoryDataTable(ds.Tables["Category"]));
            }
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
            this.tableCategory = ((CategoryDataTable)(this.Tables["Category"]));
            if ((this.tableCategory != null)) {
                this.tableCategory.InitVars();
            }
            this.tableCategoryGroup = ((CategoryGroupDataTable)(this.Tables["CategoryGroup"]));
            if ((this.tableCategoryGroup != null)) {
                this.tableCategoryGroup.InitVars();
            }
            this.relationCategoryCategoryGroup = this.Relations["CategoryCategoryGroup"];
        }
        
        private void InitClass() {
            this.DataSetName = "DataSetCategory";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DataSetCategory.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableCategory = new CategoryDataTable();
            this.Tables.Add(this.tableCategory);
            this.tableCategoryGroup = new CategoryGroupDataTable();
            this.Tables.Add(this.tableCategoryGroup);
            ForeignKeyConstraint fkc;
            fkc = new ForeignKeyConstraint("CategoryCategoryGroup", new DataColumn[] {
                        this.tableCategory.CategoryIDColumn}, new DataColumn[] {
                        this.tableCategoryGroup.CategoryIDColumn});
            this.tableCategoryGroup.Constraints.Add(fkc);
            fkc.AcceptRejectRule = AcceptRejectRule.None;
            fkc.DeleteRule = Rule.Cascade;
            fkc.UpdateRule = Rule.Cascade;
            this.relationCategoryCategoryGroup = new DataRelation("CategoryCategoryGroup", new DataColumn[] {
                        this.tableCategory.CategoryIDColumn}, new DataColumn[] {
                        this.tableCategoryGroup.CategoryIDColumn}, false);
            this.Relations.Add(this.relationCategoryCategoryGroup);
        }
        
        private bool ShouldSerializeCategory() {
            return false;
        }
        
        private bool ShouldSerializeCategoryGroup() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void CategoryRowChangeEventHandler(object sender, CategoryRowChangeEvent e);
        
        public delegate void CategoryGroupRowChangeEventHandler(object sender, CategoryGroupRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnCategoryID;
            
            private DataColumn columnCategoryParentID;
            
            private DataColumn columnCategoryName;
            
            private DataColumn columnCategoryPath;
            
            private DataColumn columnCategoryDescription;
            
            private DataColumn columnPublish;
            
            internal CategoryDataTable() : 
                    base("Category") {
                this.InitClass();
            }
            
            internal CategoryDataTable(DataTable table) : 
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
            
            internal DataColumn CategoryIDColumn {
                get {
                    return this.columnCategoryID;
                }
            }
            
            internal DataColumn CategoryParentIDColumn {
                get {
                    return this.columnCategoryParentID;
                }
            }
            
            internal DataColumn CategoryNameColumn {
                get {
                    return this.columnCategoryName;
                }
            }
            
            internal DataColumn CategoryPathColumn {
                get {
                    return this.columnCategoryPath;
                }
            }
            
            internal DataColumn CategoryDescriptionColumn {
                get {
                    return this.columnCategoryDescription;
                }
            }
            
            internal DataColumn PublishColumn {
                get {
                    return this.columnPublish;
                }
            }
            
            public CategoryRow this[int index] {
                get {
                    return ((CategoryRow)(this.Rows[index]));
                }
            }
            
            public event CategoryRowChangeEventHandler CategoryRowChanged;
            
            public event CategoryRowChangeEventHandler CategoryRowChanging;
            
            public event CategoryRowChangeEventHandler CategoryRowDeleted;
            
            public event CategoryRowChangeEventHandler CategoryRowDeleting;
            
            public void AddCategoryRow(CategoryRow row) {
                this.Rows.Add(row);
            }
            
            public CategoryRow AddCategoryRow(int CategoryParentID, string CategoryName, string CategoryPath, string CategoryDescription, bool Publish) {
                CategoryRow rowCategoryRow = ((CategoryRow)(this.NewRow()));
                rowCategoryRow.ItemArray = new object[] {
                        null,
                        CategoryParentID,
                        CategoryName,
                        CategoryPath,
                        CategoryDescription,
                        Publish};
                this.Rows.Add(rowCategoryRow);
                return rowCategoryRow;
            }
            
            public CategoryRow FindByCategoryID(int CategoryID) {
                return ((CategoryRow)(this.Rows.Find(new object[] {
                            CategoryID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CategoryDataTable cln = ((CategoryDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            internal void InitVars() {
                this.columnCategoryID = this.Columns["CategoryID"];
                this.columnCategoryParentID = this.Columns["CategoryParentID"];
                this.columnCategoryName = this.Columns["CategoryName"];
                this.columnCategoryPath = this.Columns["CategoryPath"];
                this.columnCategoryDescription = this.Columns["CategoryDescription"];
                this.columnPublish = this.Columns["Publish"];
            }
            
            private void InitClass() {
                this.columnCategoryID = new DataColumn("CategoryID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryID);
                this.columnCategoryParentID = new DataColumn("CategoryParentID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryParentID);
                this.columnCategoryName = new DataColumn("CategoryName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryName);
                this.columnCategoryPath = new DataColumn("CategoryPath", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryPath);
                this.columnCategoryDescription = new DataColumn("CategoryDescription", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCategoryDescription);
                this.columnPublish = new DataColumn("Publish", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPublish);
                this.Constraints.Add(new UniqueConstraint("DataSetCategoryKey1", new DataColumn[] {
                                this.columnCategoryID}, true));
                this.columnCategoryID.AutoIncrement = true;
                this.columnCategoryID.AllowDBNull = false;
                this.columnCategoryID.ReadOnly = true;
                this.columnCategoryID.Unique = true;
                this.columnCategoryParentID.AllowDBNull = false;
                this.columnCategoryName.AllowDBNull = false;
                this.columnPublish.AllowDBNull = false;
                this.columnPublish.DefaultValue = true;
            }
            
            public CategoryRow NewCategoryRow() {
                return ((CategoryRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CategoryRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CategoryRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CategoryRowChanged != null)) {
                    this.CategoryRowChanged(this, new CategoryRowChangeEvent(((CategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CategoryRowChanging != null)) {
                    this.CategoryRowChanging(this, new CategoryRowChangeEvent(((CategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CategoryRowDeleted != null)) {
                    this.CategoryRowDeleted(this, new CategoryRowChangeEvent(((CategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CategoryRowDeleting != null)) {
                    this.CategoryRowDeleting(this, new CategoryRowChangeEvent(((CategoryRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCategoryRow(CategoryRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryRow : DataRow {
            
            private CategoryDataTable tableCategory;
            
            internal CategoryRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCategory = ((CategoryDataTable)(this.Table));
            }
            
            public int CategoryID {
                get {
                    return ((int)(this[this.tableCategory.CategoryIDColumn]));
                }
                set {
                    this[this.tableCategory.CategoryIDColumn] = value;
                }
            }
            
            public int CategoryParentID {
                get {
                    return ((int)(this[this.tableCategory.CategoryParentIDColumn]));
                }
                set {
                    this[this.tableCategory.CategoryParentIDColumn] = value;
                }
            }
            
            public string CategoryName {
                get {
                    return ((string)(this[this.tableCategory.CategoryNameColumn]));
                }
                set {
                    this[this.tableCategory.CategoryNameColumn] = value;
                }
            }
            
            public string CategoryPath {
                get {
                    try {
                        return ((string)(this[this.tableCategory.CategoryPathColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCategory.CategoryPathColumn] = value;
                }
            }
            
            public string CategoryDescription {
                get {
                    try {
                        return ((string)(this[this.tableCategory.CategoryDescriptionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCategory.CategoryDescriptionColumn] = value;
                }
            }
            
            public bool Publish {
                get {
                    return ((bool)(this[this.tableCategory.PublishColumn]));
                }
                set {
                    this[this.tableCategory.PublishColumn] = value;
                }
            }
            
            public bool IsCategoryPathNull() {
                return this.IsNull(this.tableCategory.CategoryPathColumn);
            }
            
            public void SetCategoryPathNull() {
                this[this.tableCategory.CategoryPathColumn] = System.Convert.DBNull;
            }
            
            public bool IsCategoryDescriptionNull() {
                return this.IsNull(this.tableCategory.CategoryDescriptionColumn);
            }
            
            public void SetCategoryDescriptionNull() {
                this[this.tableCategory.CategoryDescriptionColumn] = System.Convert.DBNull;
            }
            
            public CategoryGroupRow[] GetCategoryGroupRows() {
                return ((CategoryGroupRow[])(this.GetChildRows(this.Table.ChildRelations["CategoryCategoryGroup"])));
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CategoryRowChangeEvent : EventArgs {
            
            private CategoryRow eventRow;
            
            private DataRowAction eventAction;
            
            public CategoryRowChangeEvent(CategoryRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CategoryRow Row {
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
            
            public CategoryGroupRow AddCategoryGroupRow(CategoryRow parentCategoryRowByCategoryCategoryGroup, int GroupID) {
                CategoryGroupRow rowCategoryGroupRow = ((CategoryGroupRow)(this.NewRow()));
                rowCategoryGroupRow.ItemArray = new object[] {
                        null,
                        parentCategoryRowByCategoryCategoryGroup[0],
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
                this.Constraints.Add(new UniqueConstraint("DataSetCategoryKey2", new DataColumn[] {
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
            
            public CategoryRow CategoryRow {
                get {
                    return ((CategoryRow)(this.GetParentRow(this.Table.ParentRelations["CategoryCategoryGroup"])));
                }
                set {
                    this.SetParentRow(value, this.Table.ParentRelations["CategoryCategoryGroup"]);
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
