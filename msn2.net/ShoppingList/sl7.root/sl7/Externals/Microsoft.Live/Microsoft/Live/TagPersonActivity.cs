namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class TagPersonActivity : Activity
    {
        private ActivityPhotoAlbum _ActivityContext;
        private ObservableCollection<ActivityUser> _ActivityObjects = new ObservableCollection<ActivityUser>();
        private ActivityPhoto _ActivityTarget;

        private void OnActivityContextChanged()
        {
        }

        private void OnActivityContextChanging(ActivityPhotoAlbum value)
        {
        }

        private void OnActivityTargetChanged()
        {
        }

        private void OnActivityTargetChanging(ActivityPhoto value)
        {
        }

        public ActivityPhotoAlbum ActivityContext
        {
            get
            {
                return this._ActivityContext;
            }
            set
            {
                this.OnActivityContextChanging(value);
                this._ActivityContext = value;
                this.OnActivityContextChanged();
                this.OnPropertyChanged("ActivityContext");
            }
        }

        public ObservableCollection<ActivityUser> ActivityObjects
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

        public ActivityPhoto ActivityTarget
        {
            get
            {
                return this._ActivityTarget;
            }
            set
            {
                this.OnActivityTargetChanging(value);
                this._ActivityTarget = value;
                this.OnActivityTargetChanged();
                this.OnPropertyChanged("ActivityTarget");
            }
        }

        public ActivityUser Person
        {
            get
            {
                if (this.ActivityObjects.Count != 0)
                {
                    return this.ActivityObjects[0];
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.ActivityObjects.Insert(0, value);
            }
        }

        public ActivityPhotoAlbum PhotoAlbum
        {
            get
            {
                return this.ActivityContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.ActivityContext = value;
            }
        }
    }
}

