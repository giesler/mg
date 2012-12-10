namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddBookmarkActivity : Activity
    {
        private ObservableCollection<ActivityBookmark> _ActivityObjects;

        public AddBookmarkActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityBookmark>();
        }

        private AddBookmarkActivity(ActivityBookmark bookmark)
        {
            this._ActivityObjects = new ObservableCollection<ActivityBookmark>();
            this.Bookmark = bookmark;
        }

        public AddBookmarkActivity(Uri targetLink) : this(targetLink, null)
        {
        }

        public AddBookmarkActivity(Uri targetLink, string title) : this(targetLink, title, null)
        {
        }

        public AddBookmarkActivity(Uri targetLink, string title, Uri previewLink) : this(targetLink, title, previewLink, null)
        {
        }

        public AddBookmarkActivity(Uri targetLink, string title, Uri previewLink, string description)
            : this(new ActivityBookmark()
            {
                Title = title,
                TargetLink = targetLink,
                PreviewLink = previewLink,
                Description = description
            })
        {

            if (targetLink == null)
            {
                throw new ArgumentNullException("alternateLink");
            }
        }

        public ObservableCollection<ActivityBookmark> ActivityObjects
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

        public ActivityBookmark Bookmark
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

