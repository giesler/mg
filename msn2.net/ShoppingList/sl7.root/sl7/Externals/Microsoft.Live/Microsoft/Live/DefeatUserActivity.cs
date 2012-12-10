namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class DefeatUserActivity : Activity
    {
        private ObservableCollection<ActivityUser> _ActivityObjects;
        private ActivityGame _ActivityTarget;

        public DefeatUserActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityUser>();
        }

        private DefeatUserActivity(ActivityUser user, ActivityGame game)
        {
            this._ActivityObjects = new ObservableCollection<ActivityUser>();
            this.User = user;
            this.ActivityTarget = game;
        }

        public DefeatUserActivity(Uri userProfileLink, Uri gameLink, Uri gamePreviewLink)
        {
            this._ActivityObjects = new ObservableCollection<ActivityUser>();
        }

        public DefeatUserActivity(Uri userProfileLink, Uri gameLink, Uri gamePreviewLink, string userName)
        {
            this._ActivityObjects = new ObservableCollection<ActivityUser>();
        }

        public DefeatUserActivity(Uri userProfileLink, Uri gameLink, Uri gamePreviewLink, string userDisplayName, string gameTitle)
            : this(new ActivityUser()
            {
                AlternateLink = userProfileLink,
                DisplayName = userDisplayName
            },
                new ActivityGame()
                {
                    AlternateLink = gameLink,
                    PreviewLink = gamePreviewLink,
                    Title = gameTitle
                })
        {

            if (userProfileLink == null)
            {
                throw new ArgumentNullException("userProfileLink");
            }
            if (gameLink == null)
            {
                throw new ArgumentNullException("gameLink");
            }
            if (gamePreviewLink == null)
            {
                throw new ArgumentNullException("gamePreviewLink");
            }
        }

        private void OnActivityTargetChanged()
        {
        }

        private void OnActivityTargetChanging(ActivityGame value)
        {
        }

        public ObservableCollection<ActivityUser> ActivityObjects
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

        public ActivityGame ActivityTarget
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

        public ActivityUser User
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

