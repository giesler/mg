namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddArticleActivity : Activity
    {
        private ObservableCollection<ActivityArticle> _ActivityObjects;

        public AddArticleActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityArticle>();
        }

        private AddArticleActivity(ActivityArticle article)
        {
            this._ActivityObjects = new ObservableCollection<ActivityArticle>();
            this.Article = article;
        }

        public AddArticleActivity(Uri permaLink) : this(permaLink, null)
        {
        }

        public AddArticleActivity(Uri permaLink, string title) : this(permaLink, title, null)
        {
        }

        public AddArticleActivity(Uri permaLink, string title, string summary)
            : this(new ActivityArticle()
            {
                PermaLink = permaLink,
                Title = title,
                Summary = summary
            })
        {
            if (string.IsNullOrEmpty(summary))
            {
                throw new ArgumentNullException("summary");
            }
        }

        public ObservableCollection<ActivityArticle> ActivityObjects
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

        public ActivityArticle Article
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

