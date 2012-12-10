namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddCommentActivity : Activity
    {
        private ObservableCollection<ActivityComment> _ActivityObjects;

        public AddCommentActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityComment>();
        }

        private AddCommentActivity(ActivityComment comment)
        {
            this._ActivityObjects = new ObservableCollection<ActivityComment>();
            this.Comment = comment;
        }

        public AddCommentActivity(Uri previewLink, Uri alternateLink) : this(previewLink, alternateLink, null)
        {
        }

        public AddCommentActivity(Uri previewLink, Uri alternateLink, string title) : this(previewLink, alternateLink, title, null)
        {
        }

        public AddCommentActivity(Uri previewLink, Uri alternateLink, string title, string content)
            : this(new ActivityComment()
            {
                InReplyTo = previewLink,
                AlternateLink = alternateLink,
                Title = title,
                Content = content
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

        public ObservableCollection<ActivityComment> ActivityObjects
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

        public ActivityComment Comment
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

