namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class SaveProductActivity : Activity
    {
        private ObservableCollection<ActivityProduct> _ActivityObjects;
        private ActivityList _ActivityTarget;

        public SaveProductActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
        }

        private SaveProductActivity(ActivityProduct productActivity, ActivityList productList)
        {
            this._ActivityObjects = new ObservableCollection<ActivityProduct>();
            this.Product = productActivity;
            this.ActivityTarget = productList;
        }

        public SaveProductActivity(Uri productAlternateLink, Uri listAlternateLink) : this(productAlternateLink, null, listAlternateLink, null)
        {
        }

        public SaveProductActivity(Uri productAlternateLink, string productTitle, Uri listAlternateLink, string listTitle) : this(productAlternateLink, productTitle, listAlternateLink, listTitle, null, null)
        {
        }

        public SaveProductActivity(Uri productAlternateLink, string productTitle, Uri listAlternateLink, string listTitle, Uri previewLink, string summary)
            : this(new ActivityProduct()
            {
                Title = productTitle,
                AlternateLink = productAlternateLink,
                PreviewLink = previewLink,
                Summary = summary
            },
                new ActivityList()
                {
                    Title = listTitle,
                    AlternateLink = listAlternateLink
                })
        {

            if (productAlternateLink == null)
            {
                throw new ArgumentNullException("productAlternateLink");
            }
            if (listAlternateLink == null)
            {
                throw new ArgumentNullException("listAlternateLink");
            }
        }

        private void OnActivityTargetChanged()
        {
        }

        private void OnActivityTargetChanging(ActivityList value)
        {
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

        public ActivityList ActivityTarget
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

