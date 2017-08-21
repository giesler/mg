namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CalendarTimeZoneComponent : INotifyPropertyChanged
    {
        private DateTime _DtStart;
        private double _OffsetFrom;
        private double _OffsetTo;
        private string _Rule;
        private string _Type;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public DateTime DtStart
        {
            get
            {
                return this._DtStart;
            }
            set
            {
                this._DtStart = value;
                this.OnPropertyChanged("DtStart");
            }
        }

        public double OffsetFrom
        {
            get
            {
                return this._OffsetFrom;
            }
            set
            {
                this._OffsetFrom = value;
                this.OnPropertyChanged("OffsetFrom");
            }
        }

        public double OffsetTo
        {
            get
            {
                return this._OffsetTo;
            }
            set
            {
                this._OffsetTo = value;
                this.OnPropertyChanged("OffsetTo");
            }
        }

        public string Rule
        {
            get
            {
                return this._Rule;
            }
            set
            {
                this._Rule = value;
                this.OnPropertyChanged("Rule");
            }
        }

        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
                this.OnPropertyChanged("Type");
            }
        }
    }
}

