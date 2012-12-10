namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Services.Common;

    [DataServiceKey("Id"), EntitySet("Events")]
    public class CalendarEvent : LiveResource
    {
        private string _Availability = "Busy";
        private double _EndTimeZoneOffset;
        private bool _IsAllDayEvent;
        private bool _IsMeetingRequest;
        private string _Location;
        private ObservableCollection<CalendarEventOccurrence> _Occurrences = new ObservableCollection<CalendarEventOccurrence>();
        private ObservableCollection<EventParticipant> _Participants = new ObservableCollection<EventParticipant>();
        private string _RecurrenceDescription;
        private int _ReminderTimeInMin;
        private double _StartTimeZoneOffset;
        private DateTime _UtcEndTime;
        private DateTime _UtcStartTime;
        private string _Visibility = "Public";

        public string Availability
        {
            get
            {
                return this._Availability;
            }
            set
            {
                this._Availability = value;
                this.OnPropertyChanged("Availability");
            }
        }

        public double EndTimeZoneOffset
        {
            get
            {
                return this._EndTimeZoneOffset;
            }
            internal set
            {
                this._EndTimeZoneOffset = value;
                this.OnPropertyChanged("EndTimeZoneOffset");
            }
        }

        public bool IsAllDayEvent
        {
            get
            {
                return this._IsAllDayEvent;
            }
            set
            {
                this._IsAllDayEvent = value;
                this.OnPropertyChanged("IsAllDayEvent");
            }
        }

        public bool IsMeetingRequest
        {
            get
            {
                return this._IsMeetingRequest;
            }
            internal set
            {
                this._IsMeetingRequest = value;
                this.OnPropertyChanged("IsMeetingRequest");
            }
        }

        public string Location
        {
            get
            {
                return this._Location;
            }
            set
            {
                this._Location = value;
                this.OnPropertyChanged("Location");
            }
        }

        public ObservableCollection<CalendarEventOccurrence> Occurrences
        {
            get
            {
                return this._Occurrences;
            }
            internal set
            {
                if (value != null)
                {
                    this._Occurrences = value;
                }
            }
        }

        public ObservableCollection<EventParticipant> Participants
        {
            get
            {
                return this._Participants;
            }
            set
            {
                if (value != null)
                {
                    this._Participants = value;
                }
            }
        }

        public string RecurrenceDescription
        {
            get
            {
                return this._RecurrenceDescription;
            }
            internal set
            {
                this._RecurrenceDescription = value;
                this.OnPropertyChanged("RecurrenceDescription");
            }
        }

        public int ReminderTimeInMin
        {
            get
            {
                return this._ReminderTimeInMin;
            }
            internal set
            {
                this._ReminderTimeInMin = value;
                this.OnPropertyChanged("ReminderTimeInMin");
            }
        }

        public double StartTimeZoneOffset
        {
            get
            {
                return this._StartTimeZoneOffset;
            }
            internal set
            {
                this._StartTimeZoneOffset = value;
                this.OnPropertyChanged("StartTimeZoneOffset");
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
            set
            {
                this._UtcStartTime = value;
                this.OnPropertyChanged("UtcStartTime");
            }
        }

        public string Visibility
        {
            get
            {
                return this._Visibility;
            }
            set
            {
                this._Visibility = value;
                this.OnPropertyChanged("Visibility");
            }
        }
    }
}

