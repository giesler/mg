namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class SaveAchievementActivity : Activity
    {
        private ObservableCollection<ActivityAchievement> _ActivityObjects;
        private ActivityGame _ActivityTarget;

        public SaveAchievementActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityAchievement>();
        }

        private SaveAchievementActivity(ActivityAchievement achievement, ActivityGame game)
        {
            this._ActivityObjects = new ObservableCollection<ActivityAchievement>();
            this.Achievement = achievement;
            this.ActivityTarget = game;
        }

        public SaveAchievementActivity(string achievement, Uri gameAlternateLink, Uri gamePreviewLink) : this(achievement, gameAlternateLink, gamePreviewLink, null)
        {
        }

        public SaveAchievementActivity(string achievement, Uri gameAlternateLink, Uri gamePreviewLink, string gameTitle) : this(achievement, gameAlternateLink, gamePreviewLink, gameTitle, null)
        {
        }

        public SaveAchievementActivity(string achievement, Uri gameAlternateLink, Uri gamePreviewLink, string gameTitle, string summary)
            : this(new ActivityAchievement()
            {
                Title = achievement,
                Summary = summary
            },
                new ActivityGame()
                {
                    PreviewLink = gamePreviewLink,
                    AlternateLink = gameAlternateLink,
                    Title = gameTitle
                })
        {

            if (string.IsNullOrEmpty(achievement))
            {
                throw new ArgumentNullException("achievement");
            }
            if (gameAlternateLink == null)
            {
                throw new ArgumentNullException("gameAlternateLink");
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

        public ActivityAchievement Achievement
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

        public ObservableCollection<ActivityAchievement> ActivityObjects
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
    }
}

