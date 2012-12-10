namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class RateProductActivity : Activity
    {
        private ObservableCollection<ActivityProduct> _ActivityObjects;

        public RateProductActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
        }

        private RateProductActivity(ActivityProduct product)
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
            this.Product = product;
        }

        public RateProductActivity(Uri reviewLink) : this(reviewLink, null)
        {
        }

        public RateProductActivity(Uri reviewLink, string title) : this(reviewLink, title, null)
        {
        }

        public RateProductActivity(Uri reviewLink, string title, string summary) : this(reviewLink, title, summary, null)
        {
        }

        public RateProductActivity(Uri reviewLink, string title, string summary, Uri previewLink)
            : this(new ActivityProduct()
            {
                AlternateLink = reviewLink,
                Title = title,
                PreviewLink = previewLink,
                Summary = summary
            })
        {

            if (reviewLink == null)
            {
                throw new ArgumentNullException("reviewLink");
            }
        }

        public ObservableCollection<ActivityProduct> ActivityObjects
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

        public ActivityProduct Product
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

