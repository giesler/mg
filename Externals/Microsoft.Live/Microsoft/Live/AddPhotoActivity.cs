namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class AddPhotoActivity : Activity
    {
        private ObservableCollection<ActivityPhoto> _ActivityObjects;
        private ActivityPhotoAlbum _ActivityTarget;

        public AddPhotoActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityPhoto>();
        }

        private AddPhotoActivity(ActivityPhoto photo)
        {
            this._ActivityObjects = new ObservableCollection<ActivityPhoto>();
            this.Photo = photo;
        }

        public AddPhotoActivity(Uri photoPreviewLink)
            : this(new ActivityPhoto()
            {
                PreviewLink = photoPreviewLink
            })
        {

            if (photoPreviewLink == null)
            {
                throw new ArgumentNullException("photoPreviewLink");
            }
        }

        private AddPhotoActivity(ActivityPhoto photo, ActivityPhotoAlbum album)
        {
            this._ActivityObjects = new ObservableCollection<ActivityPhoto>();
            this.Photo = photo;
            this.ActivityTarget = album;
        }

        public AddPhotoActivity(Uri albumLink, string albumTitle)
            : this(new ActivityPhoto(), new ActivityPhotoAlbum()
            {
                AlternateLink = albumLink,
                Title = albumTitle
            })
        {

            if (albumLink == null)
            {
                throw new ArgumentNullException("albumLink");
            }
        }

        public AddPhotoActivity(Uri photoPreviewLink, Uri alternateLink)
            : this(new ActivityPhoto()
            {
                PreviewLink = photoPreviewLink,
                AlternateLink = alternateLink
            })
        {

            if (photoPreviewLink == null)
            {
                throw new ArgumentNullException("photoPreviewLink");
            }
        }

        public AddPhotoActivity(Uri photoPreviewLink, Uri alternateLink, string photoTitle, string photoDescription)
            : this(new ActivityPhoto()
            {
                PreviewLink = photoPreviewLink,
                AlternateLink = alternateLink,
                Description = photoDescription,
                Title = photoTitle
            })
        {

            if (photoPreviewLink == null)
            {
                throw new ArgumentNullException("photoPreviewLink");
            }
        }

        public AddPhotoActivity(Uri photoPreviewLink, Uri alternateLink, string photoTitle, string photoDescription, Uri albumLink, string albumTitle)
            : this(new ActivityPhoto()
            {
                PreviewLink = photoPreviewLink,
                AlternateLink = alternateLink,
                Description = photoDescription,
                Title = photoTitle
            },
                new ActivityPhotoAlbum()
                {
                    AlternateLink = albumLink,
                    Title = albumTitle
                })
        {

            if (photoPreviewLink == null)
            {
                throw new ArgumentNullException("photoPreviewLink");
            }
            if (albumLink == null)
            {
                throw new ArgumentNullException("albumLink");
            }
        }

        private void OnActivityTargetChanged()
        {
        }

        private void OnActivityTargetChanging(ActivityPhotoAlbum value)
        {
        }

        public ObservableCollection<ActivityPhoto> ActivityObjects
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

        public ActivityPhotoAlbum ActivityTarget
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

        public ActivityPhoto Photo
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

