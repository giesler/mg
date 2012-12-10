namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class PlayVideoActivity : Activity
    {
        private ObservableCollection<ActivityVideo> _ActivityObjects;

        public PlayVideoActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityVideo>();
        }

        private PlayVideoActivity(ActivityVideo video)
        {
            this._ActivityObjects = new ObservableCollection<ActivityVideo>();
            this.Video = video;
        }

        public PlayVideoActivity(Uri previewLink, Uri alternateLink) : this(previewLink, alternateLink, null)
        {
        }

        public PlayVideoActivity(Uri previewLink, Uri alternateLink, string title) : this(previewLink, alternateLink, title, null)
        {
        }

        public PlayVideoActivity(Uri previewLink, Uri alternateLink, string title, string description) : this(previewLink, alternateLink, title, description, null)
        {
        }

        public PlayVideoActivity(Uri previewLink, Uri alternateLink, string title, string description, Uri enclosureLink)
            : this(new ActivityVideo()
            {
                PreviewLink = previewLink,
                AlternateLink = alternateLink,
                Description = description,
                Title = title,
                EnclosureLink = enclosureLink
            })
        {

            if (previewLink == null)
            {
                throw new ArgumentNullException("previewLink");
            }
            if (alternateLink == null)
            {
                throw new ArgumentNullException("alternateLink");
            }
        }

        public ObservableCollection<ActivityVideo> ActivityObjects
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

        public ActivityVideo Video
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
    }
}

