namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddProductActivity : Activity
    {
        private ObservableCollection<ActivityProduct> _ActivityObjects;

        public AddProductActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
        }

        private AddProductActivity(ActivityProduct product)
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
            this.Product = product;
        }

        public AddProductActivity(Uri productAlternateLink) : this(productAlternateLink, null)
        {
        }

        public AddProductActivity(Uri productAlternateLink, string title) : this(productAlternateLink, title, null)
        {
        }

        public AddProductActivity(Uri productAlternateLink, string title, string summary) : this(productAlternateLink, title, summary, null)
        {
        }

        public AddProductActivity(Uri productAlternateLink, string title, string summary, Uri previewLink)
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

