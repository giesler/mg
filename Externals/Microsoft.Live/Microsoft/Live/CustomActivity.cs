namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class CustomActivity : Activity
    {
        private ObservableCollection<ActivityCustom> _ActivityObjects;
        private ActivityCustom _ActivityTarget;
        private string _CustomActivityVerb;

        public CustomActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityCustom>();
        }

        private CustomActivity(string verb, ActivityCustom activityObject)
        {
            this._ActivityObjects = new ObservableCollection<ActivityCustom>();
            this.Verb = verb;
            this.Object = activityObject;
        }

        public CustomActivity(string verb, Uri alternateLink) : this(verb, alternateLink, null)
        {
        }

        public CustomActivity(string verb, Uri alternateLink, string title) : this(verb, alternateLink, title, null)
        {
        }

        public CustomActivity(string verb, Uri alternateLink, string title, Uri previewLink) : this(verb, alternateLink, title, previewLink, null)
        {
        }

        public CustomActivity(string verb, Uri alternateLink, string title, Uri previewLink, string summary)
            : this(verb, new ActivityCustom()
            {
                AlternateLink = alternateLink,
                PreviewLink = previewLink,
                Summary = summary,
                Title = title
            })
        {

            if (string.IsNullOrEmpty(verb))
            {
                throw new ArgumentNullException("verb");
            }
            if (alternateLink == null)
            {
                throw new ArgumentNullException("alternateLink");
            }
        }

        private void OnActivityTargetChanged()
        {
        }

        private void OnActivityTargetChanging(ActivityCustom value)
        {
        }

        private void OnCustomActivityVerbChanged()
        {
        }

        private void OnCustomActivityVerbChanging(string value)
        {
        }

        public ObservableCollection<ActivityCustom> ActivityObjects
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

        public ActivityCustom ActivityTarget
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

        public string CustomActivityVerb
        {
            get
            {
                return this._CustomActivityVerb;
            }
            set
            {
                this.OnCustomActivityVerbChanging(value);
                this._CustomActivityVerb = value;
                this.OnCustomActivityVerbChanged();
                this.OnPropertyChanged("CustomActivityVerb");
            }
        }

        public ActivityCustom Object
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

        public string Verb
        {
            get
            {
                return this.CustomActivityVerb;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.CustomActivityVerb = value;
            }
        }
    }
}

