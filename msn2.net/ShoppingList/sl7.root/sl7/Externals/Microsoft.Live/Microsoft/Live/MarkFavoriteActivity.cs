namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class MarkFavoriteActivity : Activity
    {
        private ObservableCollection<ActivityProduct> _ActivityObjects;

        public MarkFavoriteActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
        }

        private MarkFavoriteActivity(ActivityProduct product)
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
            this.Product = product;
        }

        public MarkFavoriteActivity(Uri productAlternateLink) : this(productAlternateLink, null)
        {
        }

        public MarkFavoriteActivity(Uri productAlternateLink, string title) : this(productAlternateLink, title, null)
        {
        }

        public MarkFavoriteActivity(Uri productAlternateLink, string title, string summary) : this(productAlternateLink, title, summary, null)
        {
        }

        public MarkFavoriteActivity(Uri productAlternateLink, string title, string summary, Uri previewLink)
            : this(new ActivityProduct()
            {
                AlternateLink = productAlternateLink,
                Title = title,
                PreviewLink = previewLink,
                Summary = summary
            })
        {

            if (productAlternateLink == null)
            {
                throw new ArgumentNullException("productAlternateLink");
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

