namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CalendarTimeZone : INotifyPropertyChanged
    {
        private string _Id;
        private ObservableCollection<CalendarTimeZoneComponent> _TimeZoneComponents = new ObservableCollection<CalendarTimeZoneComponent>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public ObservableCollection<CalendarTimeZoneComponent> TimeZoneComponents
        {
            get
            {
                return this._TimeZoneComponents;
            }
            set
            {
                if (value != null)
                {
                    this._TimeZoneComponents = value;
                }
            }
        }
    }
}

