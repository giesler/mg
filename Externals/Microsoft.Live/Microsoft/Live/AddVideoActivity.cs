namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddVideoActivity : Activity
    {
        private ObservableCollection<ActivityVideo> _ActivityObjects;

        public AddVideoActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityVideo>();
        }

        private AddVideoActivity(ActivityVideo video)
        {
            this._ActivityObjects = new ObservableCollection<ActivityVideo>();
            this.Video = video;
        }

        public AddVideoActivity(Uri alternateLink) : this(alternateLink, null)
        {
        }

        public AddVideoActivity(Uri alternateLink, Uri previewLink) : this(alternateLink, previewLink, null)
        {
        }

        public AddVideoActivity(Uri alternateLink, Uri previewLink, Uri enclosureLink) : this(alternateLink, previewLink, enclosureLink, null)
        {
        }

        public AddVideoActivity(Uri alternateLink, Uri previewLink, Uri enclosureLink, string videoTitle) : this(alternateLink, previewLink, enclosureLink, videoTitle, null)
        {
        }

        public AddVideoActivity(Uri alternateLink, Uri previewLink, Uri enclosureLink, string videoTitle, string videoDescription)
            : this(new ActivityVideo()
            {
                AlternateLink = alternateLink,
                PreviewLink = previewLink,
                EnclosureLink = enclosureLink,
                Title = videoTitle,
                Description = videoDescription
            })
        {

            if (alternateLink == null)
            {
                throw new ArgumentNullException("alternateLink");
            }
            if (previewLink == null)
            {
                throw new ArgumentNullException("previewLink");
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

