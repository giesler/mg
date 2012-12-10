namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddReviewActivity : Activity
    {
        private ObservableCollection<ActivityReview> _ActivityObjects;

        public AddReviewActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityReview>();
        }

        private AddReviewActivity(ActivityReview review)
        {
            this._ActivityObjects = new ObservableCollection<ActivityReview>();
            this.Review = review;
        }

        public AddReviewActivity(string title, Uri permaLink) : this(title, permaLink, null)
        {
        }

        public AddReviewActivity(string title, Uri permaLink, string content) : this(title, permaLink, content, null, null)
        {
        }

        public AddReviewActivity(string title, Uri permaLink, string content, Uri previewLink, Uri alternateLink)
            : this(new ActivityReview()
            {
                Title = title,
                PermaLink = permaLink,
                Content = content,
                AlternateLink = alternateLink,
                PreviewLink = previewLink
            })
        {

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            if (permaLink == null)
            {
                throw new ArgumentNullException("permaLink");
            }
        }

        public ObservableCollection<ActivityReview> ActivityObjects
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

        public ActivityReview Review
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

