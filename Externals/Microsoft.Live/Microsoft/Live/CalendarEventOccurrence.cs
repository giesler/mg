namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CalendarEventOccurrence : INotifyPropertyChanged
    {
        private DateTime _UtcEndTime;
        private DateTime _UtcStartTime;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public DateTime UtcEndTime
        {
            get
            {
                return this._UtcEndTime;
            }
            set
            {
                this._UtcEndTime = value;
                this.OnPropertyChanged("UtcEndTime");
            }
        }

        public DateTime UtcStartTime
        {
            get
            {
                return this._UtcStartTime;
            }
            internal set
            {
                this._UtcStartTime = value;
                this.OnPropertyChanged("UtcStartTime");
            }
        }
    }
}

