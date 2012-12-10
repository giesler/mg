﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3705.209
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace pics {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DataSetPicture : DataSet {
        
        private PictureDataTable tablePicture;
        
        public DataSetPicture() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DataSetPicture(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Picture"] != null)) {
                    this.Tables.Add(new PictureDataTable(ds.Tables["Picture"]));
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
        public PictureDataTable Picture {
            get {
                return this.tablePicture;
            }
        }
        
        public override DataSet Clone() {
            DataSetPicture cln = ((DataSetPicture)(base.Clone()));
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
            if ((ds.Tables["Picture"] != null)) {
                this.Tables.Add(new PictureDataTable(ds.Tables["Picture"]));
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
            this.tablePicture = ((PictureDataTable)(this.Tables["Picture"]));
            if ((this.tablePicture != null)) {
                this.tablePicture.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DataSetPicture";
            this.Prefix = "";
            this.Namespace = "http://schemas.msn2.net/Picture/DataSetPicture.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tablePicture = new PictureDataTable();
            this.Tables.Add(this.tablePicture);
        }
        
        private bool ShouldSerializePicture() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void PictureRowChangeEventHandler(object sender, PictureRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PictureDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPictureID;
            
            private DataColumn columnFilename;
            
            private DataColumn columnPictureDate;
            
            private DataColumn columnTitle;
            
            private DataColumn columnDescription;
            
            private DataColumn columnPublish;
            
            private DataColumn columnRating;
            
            private DataColumn columnPictureBy;
            
            private DataColumn columnPictureSort;
            
            private DataColumn columnPictureAddDate;
            
            private DataColumn columnPictureUpdateDate;
            
            internal PictureDataTable() : 
                    base("Picture") {
                this.InitClass();
            }
            
            internal PictureDataTable(DataTable table) : 
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
            
            internal DataColumn RatingColumn {
                get {
                    return this.columnRating;
                }
            }
            
            internal DataColumn PictureByColumn {
                get {
                    return this.columnPictureBy;
                }
            }
            
            internal DataColumn PictureSortColumn {
                get {
                    return this.columnPictureSort;
                }
            }
            
            internal DataColumn PictureAddDateColumn {
                get {
                    return this.columnPictureAddDate;
                }
            }
            
            internal DataColumn PictureUpdateDateColumn {
                get {
                    return this.columnPictureUpdateDate;
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
            
            public PictureRow AddPictureRow(string Filename, System.DateTime PictureDate, string Title, string Description, bool Publish, System.Byte Rating, int PictureBy, int PictureSort, System.DateTime PictureAddDate, System.DateTime PictureUpdateDate) {
                PictureRow rowPictureRow = ((PictureRow)(this.NewRow()));
                rowPictureRow.ItemArray = new object[] {
                        null,
                        Filename,
                        PictureDate,
                        Title,
                        Description,
                        Publish,
                        Rating,
                        PictureBy,
                        PictureSort,
                        PictureAddDate,
                        PictureUpdateDate};
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
            
            public override DataTable Clone() {
                PictureDataTable cln = ((PictureDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new PictureDataTable();
            }
            
            internal void InitVars() {
                this.columnPictureID = this.Columns["PictureID"];
                this.columnFilename = this.Columns["Filename"];
                this.columnPictureDate = this.Columns["PictureDate"];
                this.columnTitle = this.Columns["Title"];
                this.columnDescription = this.Columns["Description"];
                this.columnPublish = this.Columns["Publish"];
                this.columnRating = this.Columns["Rating"];
                this.columnPictureBy = this.Columns["PictureBy"];
                this.columnPictureSort = this.Columns["PictureSort"];
                this.columnPictureAddDate = this.Columns["PictureAddDate"];
                this.columnPictureUpdateDate = this.Columns["PictureUpdateDate"];
            }
            
            private void InitClass() {
                this.columnPictureID = new DataColumn("PictureID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureID);
                this.columnFilename = new DataColumn("Filename", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnFilename);
                this.columnPictureDate = new DataColumn("PictureDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureDate);
                this.columnTitle = new DataColumn("Title", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTitle);
                this.columnDescription = new DataColumn("Description", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDescription);
                this.columnPublish = new DataColumn("Publish", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPublish);
                this.columnRating = new DataColumn("Rating", typeof(System.Byte), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnRating);
                this.columnPictureBy = new DataColumn("PictureBy", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureBy);
                this.columnPictureSort = new DataColumn("PictureSort", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureSort);
                this.columnPictureAddDate = new DataColumn("PictureAddDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureAddDate);
                this.columnPictureUpdateDate = new DataColumn("PictureUpdateDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPictureUpdateDate);
                this.Constraints.Add(new UniqueConstraint("DataSetPictureKey1", new DataColumn[] {
                                this.columnPictureID}, true));
                this.columnPictureID.AutoIncrement = true;
                this.columnPictureID.AllowDBNull = false;
                this.columnPictureID.ReadOnly = true;
                this.columnPictureID.Unique = true;
                this.columnPictureSort.AllowDBNull = false;
            }
            
            public PictureRow NewPictureRow() {
                return ((PictureRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
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
        
        [System.Diagnostics.DebuggerStepThrough()]
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
            
            public System.Byte Rating {
                get {
                    try {
                        return ((System.Byte)(this[this.tablePicture.RatingColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.RatingColumn] = value;
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
            
            public int PictureSort {
                get {
                    return ((int)(this[this.tablePicture.PictureSortColumn]));
                }
                set {
                    this[this.tablePicture.PictureSortColumn] = value;
                }
            }
            
            public System.DateTime PictureAddDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tablePicture.PictureAddDateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.PictureAddDateColumn] = value;
                }
            }
            
            public System.DateTime PictureUpdateDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tablePicture.PictureUpdateDateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePicture.PictureUpdateDateColumn] = value;
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
            
            public bool IsRatingNull() {
                return this.IsNull(this.tablePicture.RatingColumn);
            }
            
            public void SetRatingNull() {
                this[this.tablePicture.RatingColumn] = System.Convert.DBNull;
            }
            
            public bool IsPictureByNull() {
                return this.IsNull(this.tablePicture.PictureByColumn);
            }
            
            public void SetPictureByNull() {
                this[this.tablePicture.PictureByColumn] = System.Convert.DBNull;
            }
            
            public bool IsPictureAddDateNull() {
                return this.IsNull(this.tablePicture.PictureAddDateColumn);
            }
            
            public void SetPictureAddDateNull() {
                this[this.tablePicture.PictureAddDateColumn] = System.Convert.DBNull;
            }
            
            public bool IsPictureUpdateDateNull() {
                return this.IsNull(this.tablePicture.PictureUpdateDateColumn);
            }
            
            public void SetPictureUpdateDateNull() {
                this[this.tablePicture.PictureUpdateDateColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PictureRowChangeEvent : EventArgs {
            
            private PictureRow eventRow;
            
            private DataRowAction eventAction;
            
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
    }
}
