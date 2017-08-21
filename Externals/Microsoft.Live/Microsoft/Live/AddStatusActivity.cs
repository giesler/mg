namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddStatusActivity : Activity
    {
        private ObservableCollection<ActivityStatus> _ActivityObjects;

        public AddStatusActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityStatus>();
        }

        public AddStatusActivity(string status)
        {
            this._ActivityObjects = new ObservableCollection<ActivityStatus>();
            if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }
            this.Status = status;
        }

        public ObservableCollection<ActivityStatus> ActivityObjects
        {
            get
            {
                return this._ActivityObjects;
            }
            set
            {
                if (value != null)
                {
                    this._ActivityObjects = value;
                    this.OnPropertyChanged("ActivityObjects");
                }
            }
        }

        public string Status
        {
            get
            {
                if (this.ActivityObjects.Count != 0)
                {
                    return this.ActivityObjects[0].Content;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                ActivityStatus item = new ActivityStatus();
                item.Content = value;
                this.ActivityObjects.Insert(0, item);
            }
        }
    }
}

