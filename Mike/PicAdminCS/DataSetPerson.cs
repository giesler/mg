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
    public class DataSetPerson : System.Data.DataSet {
        
        private PersonDataTable tablePerson;
        
        public DataSetPerson() {
            this.InitClass();
        }
        
        private DataSetPerson(SerializationInfo info, StreamingContext context) {
            this.InitClass();
            this.GetSerializationData(info, context);
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PersonDataTable Person {
            get {
                return this.tablePerson;
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
            this.DataSetName = "DataSetPerson";
            this.Namespace = "http://www.tempuri.org/DataSetPerson.xsd";
            this.tablePerson = new PersonDataTable();
            this.Tables.Add(this.tablePerson);
        }
        
        private bool ShouldSerializePerson() {
            return false;
        }
        
        public delegate void PersonRowChangeEventHandler(object sender, PersonRowChangeEvent e);
        
        public class PersonDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPersonID;
            
            private DataColumn columnLastName;
            
            private DataColumn columnFirstName;
            
            private DataColumn columnFullName;
            
            internal PersonDataTable() : 
                    base("Person") {
                this.InitClass();
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn PersonIDColumn {
                get {
                    return this.columnPersonID;
                }
            }
            
            internal DataColumn LastNameColumn {
                get {
                    return this.columnLastName;
                }
            }
            
            internal DataColumn FirstNameColumn {
                get {
                    return this.columnFirstName;
                }
            }
            
            internal DataColumn FullNameColumn {
                get {
                    return this.columnFullName;
                }
            }
            
            public PersonRow this[int index] {
                get {
                    return ((PersonRow)(this.Rows[index]));
                }
            }
            
            public event PersonRowChangeEventHandler PersonRowChanged;
            
            public event PersonRowChangeEventHandler PersonRowChanging;
            
            public event PersonRowChangeEventHandler PersonRowDeleted;
            
            public event PersonRowChangeEventHandler PersonRowDeleting;
            
            public void AddPersonRow(PersonRow row) {
                this.Rows.Add(row);
            }
            
            public PersonRow AddPersonRow(string LastName, string FirstName, string FullName) {
                PersonRow rowPersonRow = ((PersonRow)(this.NewRow()));
                rowPersonRow.ItemArray = new object[] {
                        null,
                        LastName,
                        FirstName,
                        FullName};
                this.Rows.Add(rowPersonRow);
                return rowPersonRow;
            }
            
            public PersonRow FindByPersonID(int PersonID) {
                return ((PersonRow)(this.Rows.Find(new object[] {
                            PersonID})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            private void InitClass() {
                this.columnPersonID = new DataColumn("PersonID", typeof(int), "", System.Data.MappingType.Element);
                this.columnPersonID.AutoIncrement = true;
                this.columnPersonID.AllowDBNull = false;
                this.columnPersonID.ReadOnly = true;
                this.columnPersonID.Unique = true;
                this.Columns.Add(this.columnPersonID);
                this.columnLastName = new DataColumn("LastName", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnLastName);
                this.columnFirstName = new DataColumn("FirstName", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnFirstName);
                this.columnFullName = new DataColumn("FullName", typeof(string), "", System.Data.MappingType.Element);
                this.Columns.Add(this.columnFullName);
                this.PrimaryKey = new DataColumn[] {
                        this.columnPersonID};
            }
            
            public PersonRow NewPersonRow() {
                return ((PersonRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                // We need to ensure that all Rows in the tabled are typed rows.
                // Table calls newRow whenever it needs to create a row.
                // So the following conditions are covered by Row newRow(Record record)
                // * Cursor calls table.addRecord(record) 
                // * table.addRow(object[] values) calls newRow(record)    
                return new PersonRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PersonRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PersonRowChanged != null)) {
                    this.PersonRowChanged(this, new PersonRowChangeEvent(((PersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PersonRowChanging != null)) {
                    this.PersonRowChanging(this, new PersonRowChangeEvent(((PersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PersonRowDeleted != null)) {
                    this.PersonRowDeleted(this, new PersonRowChangeEvent(((PersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PersonRowDeleting != null)) {
                    this.PersonRowDeleting(this, new PersonRowChangeEvent(((PersonRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePersonRow(PersonRow row) {
                this.Rows.Remove(row);
            }
        }
        
        public class PersonRow : DataRow {
            
            private PersonDataTable tablePerson;
            
            internal PersonRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePerson = ((PersonDataTable)(this.Table));
            }
            
            public int PersonID {
                get {
                    return ((int)(this[this.tablePerson.PersonIDColumn]));
                }
                set {
                    this[this.tablePerson.PersonIDColumn] = value;
                }
            }
            
            public string LastName {
                get {
                    try {
                        return ((string)(this[this.tablePerson.LastNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePerson.LastNameColumn] = value;
                }
            }
            
            public string FirstName {
                get {
                    try {
                        return ((string)(this[this.tablePerson.FirstNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePerson.FirstNameColumn] = value;
                }
            }
            
            public string FullName {
                get {
                    try {
                        return ((string)(this[this.tablePerson.FullNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePerson.FullNameColumn] = value;
                }
            }
            
            public bool IsLastNameNull() {
                return this.IsNull(this.tablePerson.LastNameColumn);
            }
            
            public void SetLastNameNull() {
                this[this.tablePerson.LastNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsFirstNameNull() {
                return this.IsNull(this.tablePerson.FirstNameColumn);
            }
            
            public void SetFirstNameNull() {
                this[this.tablePerson.FirstNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsFullNameNull() {
                return this.IsNull(this.tablePerson.FullNameColumn);
            }
            
            public void SetFullNameNull() {
                this[this.tablePerson.FullNameColumn] = System.Convert.DBNull;
            }
        }
        
        public class PersonRowChangeEvent : EventArgs {
            
            private PersonRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            public PersonRowChangeEvent(PersonRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PersonRow Row {
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
