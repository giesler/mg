﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.2914.16
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
    public class DataSetPicture : System.Data.DataSet {
        
        private PictureDataTable tablePicture;
        
        private PictureCategoryDataTable tablePictureCategory;
        
        private PicturePersonDataTable tablePicturePerson;
        
        private DataRelation relationPicturePictureCategory;
        
        private DataRelation relationPicturePicturePerson;
        
        public DataSetPicture() {
            this.InitClass();
        }
        
        private DataSetPicture(SerializationInfo info, StreamingContext context) {
            this.InitClass();
            this.GetSerializationData(info, context);
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PictureDataTable Picture {
            get {
                return this.tablePicture;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PictureCategoryDataTable PictureCategory {
            get {
                return this.tablePictureCategory;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PicturePersonDataTable PicturePerson {
            get {
                return this.tablePicturePerson;
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
            this.DataSetName = "DataSetPicture";
            this.Namespace = "http://www.tempuri.org/DataSetPicture.xsd";
            this.tablePicture = new PictureDataTable();
            this.Tables.Add(this.tablePicture);
            this.tablePictureCategory = new PictureCategoryDataTable();
            this.Tables.Add(this.tablePictureCategory);
            this.tablePicturePerson = new PicturePersonDataTable();
            this.Tables.Add(this.tablePicturePerson);
            this.tablePictureCategory.Constraints.Add(new System.Data.ForeignKeyConstraint("PicturePictureCategory", new DataColumn[] {
                            this.tablePicture.PictureIDColumn}, new DataColumn[] {
                            this.tablePictureCategory.PictureIDColumn}));
            this.tablePicturePerson.Constraints.Add(new System.Data.ForeignKeyConstraint("PicturePicturePerson", new DataColumn[] {
                            this.tablePicture.PictureIDColumn}, new DataColumn[] {
                            this.tablePicturePerson.PictureIDColumn}));
            this.relationPicturePictureCategory = new DataRelation("PicturePictureCategory", new DataColumn[] {
                        this.tablePicture.PictureIDColumn}, new DataColumn[] {
                        this.tablePictureCategory.PictureIDColumn}, false);
            this.Relations.Add(this.relationPicturePictureCategory);
            this.relationPicturePicturePerson = new DataRelation("PicturePicturePerson", new DataColumn[] {
                        this.tablePicture.PictureIDColumn}, new DataColumn[] {
                        this.tablePicturePerson.PictureIDColumn}, false);
            this.Relations.Add(this.relationPicturePicturePerson);
        }
        
        private bool ShouldSerializePicture() {
            return false;
        }
        
        private bool ShouldSerializePictureCategory() {
            return false;
        }
        
        private bool ShouldSerializePicturePerson() {
            return false;
        }
        
        public delegate void PictureRowChangeEventHandler(object sender, PictureRowChangeEvent e);
        
        public delegate void PictureCategoryRowChangeEventHandler(object sender, PictureCategoryRowChangeEvent e);
        
        public delegate void PicturePersonRowChangeEventHandler(object sender, PicturePersonRowChangeEvent e);
        
        public class PictureDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPictureID;
            
            private DataColumn columnFilename;
            
            private DataColumn columnPictureDate;
            
            private DataColumn columnTitle;
            
            private DataColumn columnDescription;
            
            private DataColumn columnPublish;
            
            private DataColumn columnPictureBy;
            
            internal PictureDataTable() : 
                    base("Picture") {
                this.InitClass();
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn PictureIDColumn {
                get {
                    return this.columnPictureID;
                }
            }
            
            internal DataColumn FilenameColumn {
                get {
                    return this.columnFilename;
                }
            }
            
            internal DataColumn PictureDateColumn {
                get {
                    return this.columnPictureDate;
                }
            }
            
            internal DataColumn TitleColumn {
                get {
                    return this.columnTitle;
                }
            }
            
            internal DataColumn DescriptionColumn {
                get {
                    return this.columnDescription;
                }
            }
            
            internal DataColumn PublishColumn {
                get {
                    return this.columnPublish;
                }
            }
            
            internal DataColumn PictureByColumn {
                get {
                    return this.columnPictureBy;
                }
            }
            
            public PictureRow this[int index] {
                get {
                    return ((PictureRow)(this.Rows[index]));
                }
            }
            
            public event PictureRowChangeEventHandler PictureRowChanged;
            
            public event PictureRowChangeEventHandler PictureRowChanging;
            
            public event PictureRowChangeEventHandler PictureRowDeleted;
            
            public event PictureRowChangeEventHandler PictureRowDeleting;
            
            public void AddPictureRow(PictureRow row) {
                this.Rows.Add(row);
            }
            
            public PictureRow AddPictureRow(string Filename, System.DateTime PictureDate, string Title, string Description, bool Publish, int PictureBy) {
                PictureRow rowPictureRow = ((PictureRow)(this.NewRow()));
                rowPictureRow.ItemArray = new object[] {
                        null,
                        Filename,
                        PictureDate,
                        Title,
                        Description,
                        Publish,
                        PictureBy};
                this.Rows.Add(rowPictureRow);
                return rowPictureRow;
            }
            
            public PictureRow FindByPictureID(int PictureID) {
                return ((PictureRow)(this.Rows.Find(new object[] {
                            PictureID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            private void InitClass() {
                this.columnPictureID = new DataColumn("PictureID", typeof(int), "", System.Data.MappingType.Element);
                this.columnPictureID.AutoIncrement = true;
                this.columnPictureID.AllowDBNull = false;
                this.columnPictureID.ReadOnly = true;
                this.columnPictureID.Unique = true;
                this.Columns.Add(this.columnPictureID);
                this.columnFilename = new DataColumn("Filename", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnFilename);
                this.columnPictureDate = new DataColumn("PictureDate", typeof(System.DateTime), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureDate);
                this.columnTitle = new DataColumn("Title", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnTitle);
                this.columnDescription = new DataColumn("Description", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnDescription);
                this.columnPublish = new DataColumn("Publish", typeof(bool), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnPublish);
                this.columnPictureBy = new DataColumn("PictureBy", typeof(int), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureBy);
                this.PrimaryKey = new DataColumn[] {
                        this.columnPictureID};
            }
            
            public PictureRow NewPictureRow() {
                return ((PictureRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                // We need to ensure that all Rows in the tabled are typed rows.
                // Table calls newRow whenever it needs to create a row.
                // So the following conditions are covered by Row newRow(Record record)
                // * Cursor calls table.addRecord(record) 
                // * table.addRow(object[] values) calls newRow(record)    
                return new PictureRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PictureRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PictureRowChanged != null)) {
                    this.PictureRowChanged(this, new PictureRowChangeEvent(((PictureRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PictureRowChanging != null)) {
                    this.PictureRowChanging(this, new PictureRowChangeEvent(((PictureRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PictureRowDeleted != null)) {
                    this.PictureRowDeleted(this, new PictureRowChangeEvent(((PictureRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PictureRowDeleting != null)) {
                    this.PictureRowDeleting(this, new PictureRowChangeEvent(((PictureRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePictureRow(PictureRow row) {
                this.Rows.Remove(row);
            }
        }
        
        public class PictureRow : DataRow {
            
            private PictureDataTable tablePicture;
            
            internal PictureRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePicture = ((PictureDataTable)(this.Table));
            }
            
            public int PictureID {
                get {
                    return ((int)(this[this.tablePicture.PictureIDColumn]));
                }
                set {
                    this[this.tablePicture.PictureIDColumn] = value;
                }
            }
            
            public string Filename {
                get {
                    try {
                        return ((string)(this[this.tablePicture.FilenameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.FilenameColumn] = value;
                }
            }
            
            public System.DateTime PictureDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tablePicture.PictureDateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.PictureDateColumn] = value;
                }
            }
            
            public string Title {
                get {
                    try {
                        return ((string)(this[this.tablePicture.TitleColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.TitleColumn] = value;
                }
            }
            
            public string Description {
                get {
                    try {
                        return ((string)(this[this.tablePicture.DescriptionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.DescriptionColumn] = value;
                }
            }
            
            public bool Publish {
                get {
                    try {
                        return ((bool)(this[this.tablePicture.PublishColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.PublishColumn] = value;
                }
            }
            
            public int PictureBy {
                get {
                    try {
                        return ((int)(this[this.tablePicture.PictureByColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.PictureByColumn] = value;
                }
            }
            
            public bool IsFilenameNull() {
                return this.IsNull(this.tablePicture.FilenameColumn);
            }
            
            public void SetFilenameNull() {
                this[this.tablePicture.FilenameColumn] = System.Convert.DBNull;
            }
            
            public bool IsPictureDateNull() {
                return this.IsNull(this.tablePicture.PictureDateColumn);
            }
            
            public void SetPictureDateNull() {
                this[this.tablePicture.PictureDateColumn] = System.Convert.DBNull;
            }
            
            public bool IsTitleNull() {
                return this.IsNull(this.tablePicture.TitleColumn);
            }
            
            public void SetTitleNull() {
                this[this.tablePicture.TitleColumn] = System.Convert.DBNull;
            }
            
            public bool IsDescriptionNull() {
                return this.IsNull(this.tablePicture.DescriptionColumn);
            }
            
            public void SetDescriptionNull() {
                this[this.tablePicture.DescriptionColumn] = System.Convert.DBNull;
            }
            
            public bool IsPublishNull() {
                return this.IsNull(this.tablePicture.PublishColumn);
            }
            
            public void SetPublishNull() {
                this[this.tablePicture.PublishColumn] = System.Convert.DBNull;
            }
            
            public bool IsPictureByNull() {
                return this.IsNull(this.tablePicture.PictureByColumn);
            }
            
            public void SetPictureByNull() {
                this[this.tablePicture.PictureByColumn] = System.Convert.DBNull;
            }
            
            public PictureCategoryRow[] GetPictureCategoryRows() {
                return ((PictureCategoryRow[])(this.GetChildRows(this.Table.ChildRelations["PicturePictureCategory"])));
            }
            
            public PicturePersonRow[] GetPicturePersonRows() {
                return ((PicturePersonRow[])(this.GetChildRows(this.Table.ChildRelations["PicturePicturePerson"])));
            }
        }
        
        public class PictureRowChangeEvent : EventArgs {
            
            private PictureRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            public PictureRowChangeEvent(PictureRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PictureRow Row {
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
        
        public class PictureCategoryDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPictureID;
            
            private DataColumn columnCategoryID;
            
            internal PictureCategoryDataTable() : 
                    base("PictureCategory") {
                this.InitClass();
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn PictureIDColumn {
                get {
                    return this.columnPictureID;
                }
            }
            
            internal DataColumn CategoryIDColumn {
                get {
                    return this.columnCategoryID;
                }
            }
            
            public PictureCategoryRow this[int index] {
                get {
                    return ((PictureCategoryRow)(this.Rows[index]));
                }
            }
            
            public event PictureCategoryRowChangeEventHandler PictureCategoryRowChanged;
            
            public event PictureCategoryRowChangeEventHandler PictureCategoryRowChanging;
            
            public event PictureCategoryRowChangeEventHandler PictureCategoryRowDeleted;
            
            public event PictureCategoryRowChangeEventHandler PictureCategoryRowDeleting;
            
            public void AddPictureCategoryRow(PictureCategoryRow row) {
                this.Rows.Add(row);
            }
            
            public PictureCategoryRow AddPictureCategoryRow(PictureRow parentPictureRowByPicturePictureCategory, int CategoryID) {
                PictureCategoryRow rowPictureCategoryRow = ((PictureCategoryRow)(this.NewRow()));
                rowPictureCategoryRow.ItemArray = new object[] {
                        parentPictureRowByPicturePictureCategory[0],
                        CategoryID};
                this.Rows.Add(rowPictureCategoryRow);
                return rowPictureCategoryRow;
            }
            
            public PictureCategoryRow FindByPictureIDCategoryID(int PictureID, int CategoryID) {
                return ((PictureCategoryRow)(this.Rows.Find(new object[] {
                            PictureID,
                            CategoryID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            private void InitClass() {
                this.columnPictureID = new DataColumn("PictureID", typeof(int), "", System.Data.MappingType.Element);
                this.columnPictureID.AllowDBNull = false;
                this.Columns.Add(this.columnPictureID);
                this.columnCategoryID = new DataColumn("CategoryID", typeof(int), "", System.Data.MappingType.Element);
                this.columnCategoryID.AllowDBNull = false;
                this.Columns.Add(this.columnCategoryID);
                this.PrimaryKey = new DataColumn[] {
                        this.columnPictureID,
                        this.columnCategoryID};
            }
            
            public PictureCategoryRow NewPictureCategoryRow() {
                return ((PictureCategoryRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                // We need to ensure that all Rows in the tabled are typed rows.
                // Table calls newRow whenever it needs to create a row.
                // So the following conditions are covered by Row newRow(Record record)
                // * Cursor calls table.addRecord(record) 
                // * table.addRow(object[] values) calls newRow(record)    
                return new PictureCategoryRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PictureCategoryRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PictureCategoryRowChanged != null)) {
                    this.PictureCategoryRowChanged(this, new PictureCategoryRowChangeEvent(((PictureCategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PictureCategoryRowChanging != null)) {
                    this.PictureCategoryRowChanging(this, new PictureCategoryRowChangeEvent(((PictureCategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PictureCategoryRowDeleted != null)) {
                    this.PictureCategoryRowDeleted(this, new PictureCategoryRowChangeEvent(((PictureCategoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PictureCategoryRowDeleting != null)) {
                    this.PictureCategoryRowDeleting(this, new PictureCategoryRowChangeEvent(((PictureCategoryRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePictureCategoryRow(PictureCategoryRow row) {
                this.Rows.Remove(row);
            }
        }
        
        public class PictureCategoryRow : DataRow {
            
            private PictureCategoryDataTable tablePictureCategory;
            
            internal PictureCategoryRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePictureCategory = ((PictureCategoryDataTable)(this.Table));
            }
            
            public int PictureID {
                get {
                    return ((int)(this[this.tablePictureCategory.PictureIDColumn]));
                }
                set {
                    this[this.tablePictureCategory.PictureIDColumn] = value;
                }
            }
            
            public int CategoryID {
                get {
                    return ((int)(this[this.tablePictureCategory.CategoryIDColumn]));
                }
                set {
                    this[this.tablePictureCategory.CategoryIDColumn] = value;
                }
            }
            
            public PictureRow PictureRow {
                get {
                    return ((PictureRow)(this.GetParentRow(this.Table.ParentRelations["PicturePictureCategory"])));
                }
                set {
                    this.SetParentRow(value, this.Table.ParentRelations["PicturePictureCategory"]);
                }
            }
        }
        
        public class PictureCategoryRowChangeEvent : EventArgs {
            
            private PictureCategoryRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            public PictureCategoryRowChangeEvent(PictureCategoryRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PictureCategoryRow Row {
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
        
        public class PicturePersonDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPictureID;
            
            private DataColumn columnPersonID;
            
            internal PicturePersonDataTable() : 
                    base("PicturePerson") {
                this.InitClass();
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn PictureIDColumn {
                get {
                    return this.columnPictureID;
                }
            }
            
            internal DataColumn PersonIDColumn {
                get {
                    return this.columnPersonID;
                }
            }
            
            public PicturePersonRow this[int index] {
                get {
                    return ((PicturePersonRow)(this.Rows[index]));
                }
            }
            
            public event PicturePersonRowChangeEventHandler PicturePersonRowChanged;
            
            public event PicturePersonRowChangeEventHandler PicturePersonRowChanging;
            
            public event PicturePersonRowChangeEventHandler PicturePersonRowDeleted;
            
            public event PicturePersonRowChangeEventHandler PicturePersonRowDeleting;
            
            public void AddPicturePersonRow(PicturePersonRow row) {
                this.Rows.Add(row);
            }
            
            public PicturePersonRow AddPicturePersonRow(PictureRow parentPictureRowByPicturePicturePerson, int PersonID) {
                PicturePersonRow rowPicturePersonRow = ((PicturePersonRow)(this.NewRow()));
                rowPicturePersonRow.ItemArray = new object[] {
                        parentPictureRowByPicturePicturePerson[0],
                        PersonID};
                this.Rows.Add(rowPicturePersonRow);
                return rowPicturePersonRow;
            }
            
            public PicturePersonRow FindByPictureIDPersonID(int PictureID, int PersonID) {
                return ((PicturePersonRow)(this.Rows.Find(new object[] {
                            PictureID,
                            PersonID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            private void InitClass() {
                this.columnPictureID = new DataColumn("PictureID", typeof(int), "", System.Data.MappingType.Element);
                this.columnPictureID.AllowDBNull = false;
                this.Columns.Add(this.columnPictureID);
                this.columnPersonID = new DataColumn("PersonID", typeof(int), "", System.Data.MappingType.Element);
                this.columnPersonID.AllowDBNull = false;
                this.Columns.Add(this.columnPersonID);
                this.PrimaryKey = new DataColumn[] {
                        this.columnPictureID,
                        this.columnPersonID};
            }
            
            public PicturePersonRow NewPicturePersonRow() {
                return ((PicturePersonRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                // We need to ensure that all Rows in the tabled are typed rows.
                // Table calls newRow whenever it needs to create a row.
                // So the following conditions are covered by Row newRow(Record record)
                // * Cursor calls table.addRecord(record) 
                // * table.addRow(object[] values) calls newRow(record)    
                return new PicturePersonRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PicturePersonRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PicturePersonRowChanged != null)) {
                    this.PicturePersonRowChanged(this, new PicturePersonRowChangeEvent(((PicturePersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PicturePersonRowChanging != null)) {
                    this.PicturePersonRowChanging(this, new PicturePersonRowChangeEvent(((PicturePersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PicturePersonRowDeleted != null)) {
                    this.PicturePersonRowDeleted(this, new PicturePersonRowChangeEvent(((PicturePersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PicturePersonRowDeleting != null)) {
                    this.PicturePersonRowDeleting(this, new PicturePersonRowChangeEvent(((PicturePersonRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePicturePersonRow(PicturePersonRow row) {
                this.Rows.Remove(row);
            }
        }
        
        public class PicturePersonRow : DataRow {
            
            private PicturePersonDataTable tablePicturePerson;
            
            internal PicturePersonRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePicturePerson = ((PicturePersonDataTable)(this.Table));
            }
            
            public int PictureID {
                get {
                    return ((int)(this[this.tablePicturePerson.PictureIDColumn]));
                }
                set {
                    this[this.tablePicturePerson.PictureIDColumn] = value;
                }
            }
            
            public int PersonID {
                get {
                    return ((int)(this[this.tablePicturePerson.PersonIDColumn]));
                }
                set {
                    this[this.tablePicturePerson.PersonIDColumn] = value;
                }
            }
            
            public PictureRow PictureRow {
                get {
                    return ((PictureRow)(this.GetParentRow(this.Table.ParentRelations["PicturePicturePerson"])));
                }
                set {
                    this.SetParentRow(value, this.Table.ParentRelations["PicturePicturePerson"]);
                }
            }
        }
        
        public class PicturePersonRowChangeEvent : EventArgs {
            
            private PicturePersonRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            public PicturePersonRowChangeEvent(PicturePersonRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PicturePersonRow Row {
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
