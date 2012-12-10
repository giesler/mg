namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [EntitySet("Calendars"), DataServiceKey("Id")]
    public class Calendar : LiveResource
    {
        private LiveDataServiceCollection<CalendarEvent> _Events;
        private bool _IsDefaultCalendar;
        private bool _IsShared;
        private string _SharingPermission = "Owner";
        private Uri _SubscribableUrl;
        private CalendarTimeZone _TimeZone;
        private string _Type = "Normal";

        public LiveDataServiceCollection<CalendarEvent> Events
        {
            get
            {
                if (this._Events == null)
                {
                    this._Events = new LiveDataServiceCollection<CalendarEvent>(this);
                }
                return this._Events;
            }
        }

        public bool IsDefaultCalendar
        {
            get
            {
                return this._IsDefaultCalendar;
            }
            internal set
            {
                this._IsDefaultCalendar = value;
                this.OnPropertyChanged("IsDefaultCalendar");
            }
        }

        public bool IsShared
        {
            get
            {
                return this._IsShared;
            }
            internal set
            {
                this._IsShared = value;
                this.OnPropertyChanged("IsShared");
            }
        }

        public string SharingPermission
        {
            get
            {
                return this._SharingPermission;
            }
            internal set
            {
                this._SharingPermission = value;
                this.OnPropertyChanged("SharingPermission");
            }
        }

        public Uri SubscribableUrl
        {
            get
            {
                return this._SubscribableUrl;
            }
            internal set
            {
                this._SubscribableUrl = value;
                this.OnPropertyChanged("SubscribableUrl");
            }
        }

        public CalendarTimeZone TimeZone
        {
            get
            {
                return this._TimeZone;
            }
            internal set
            {
                this._TimeZone = value;
                this.OnPropertyChanged("TimeZone");
            }
        }

        public string Type
        {
            get
            {
                return this._Type;
            }
            internal set
            {
                this._Type = value;
                this.OnPropertyChanged("Type");
            }
        }
    }
}

