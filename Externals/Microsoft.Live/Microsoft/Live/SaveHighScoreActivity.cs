namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;

    public class SaveHighScoreActivity : Activity
    {
        private ObservableCollection<ActivityHighScore> _ActivityObjects;
        private ActivityGame _ActivityTarget;

        public SaveHighScoreActivity()
        {
            this._ActivityObjects = new ObservableCollection<ActivityHighScore>();
        }

        private SaveHighScoreActivity(ActivityHighScore highScore, ActivityGame game)
        {
            this._ActivityObjects = new ObservableCollection<ActivityHighScore>();
            this.HighScore = highScore;
            this.ActivityTarget = game;
        }

        public SaveHighScoreActivity(string title, Uri gameAlternateLink, Uri gamePreviewLink) : this(title, gameAlternateLink, gamePreviewLink, null)
        {
        }

        public SaveHighScoreActivity(string title, Uri gameAlternateLink, Uri gamePreviewLink, string gameTitle) : this(title, gameAlternateLink, gamePreviewLink, gameTitle, null)
        {
        }

        public SaveHighScoreActivity(string title, Uri gameAlternateLink, Uri gamePreviewLink, string gameTitle, string summary)
            : this(new ActivityHighScore()
            {
                Title = title,
                Summary = summary
            },
                new ActivityGame()
                {
                    Title = gameTitle,
                    AlternateLink = gameAlternateLink,
                    PreviewLink = gamePreviewLink
                })
        {

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
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

        public ObservableCollection<ActivityHighScore> ActivityObjects
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

        public ActivityHighScore HighScore
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

