namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class EventParticipant : INotifyPropertyChanged
    {
        private string _Email;
        private bool _IsOrganizer;
        private string _Name;
        private string _Role = "Required";
        private string _Status = "Accepted";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = value;
                this.OnPropertyChanged("Email");
            }
        }

        public bool IsOrganizer
        {
            get
            {
                return this._IsOrganizer;
            }
            set
            {
                this._IsOrganizer = value;
                this.OnPropertyChanged("IsOrganizer");
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public string Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                this._Role = value;
                this.OnPropertyChanged("Role");
            }
        }

        public string Status
        {
            get
            {
                return this._Status;
            }
            internal set
            {
                this._Status = value;
                this.OnPropertyChanged("Status");
            }
        }
    }
}

