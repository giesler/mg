namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [EntitySet("MyActivities")]
    public abstract class Activity : LiveResource
    {
        private ActivityUser _ActivityActor;
        private string _ActivityVerb;
        private string _ApplicationId;
        private string _ApplicationLink;
        private string _ApplicationName;
        private string _HtmlContent;
        private string _HtmlSummary;
        private DateTime _LastReplyTime;
        private int _RepliesCount;
        private LiveDataServiceCollection<AddCommentActivity> trackedReplies;

        protected Activity()
        {
        }

        public void AddReply(string reply)
        {
            if (string.IsNullOrEmpty(reply))
            {
                throw new ArgumentNullException("reply");
            }
            AddCommentActivity target = new AddCommentActivity();
            ActivityComment item = new ActivityComment();
            item.Content = reply;
            target.ActivityObjects.Add(item);
            base.GetDataContext().AddRelatedObject(this, "replies", target);
        }

        private void OnActivityActorChanged()
        {
        }

        private void OnActivityActorChanging(ActivityUser value)
        {
        }

        private void OnActivityVerbChanged()
        {
        }

        private void OnActivityVerbChanging(string value)
        {
        }

        private void OnApplicationIdChanged()
        {
        }

        private void OnApplicationIdChanging(string value)
        {
        }

        private void OnApplicationLinkChanged()
        {
        }

        private void OnApplicationLinkChanging(string value)
        {
        }

        private void OnApplicationNameChanged()
        {
        }

        private void OnApplicationNameChanging(string value)
        {
        }

        private void OnHtmlContentChanged()
        {
        }

        private void OnHtmlContentChanging(string value)
        {
        }

        private void OnHtmlSummaryChanged()
        {
        }

        private void OnHtmlSummaryChanging(string value)
        {
        }

        private void OnLastReplyTimeChanged()
        {
        }

        private void OnLastReplyTimeChanging(DateTime value)
        {
        }

        private void OnRepliesCountChanged()
        {
        }

        private void OnRepliesCountChanging(int value)
        {
        }

        private void OnRepliesLinkChanged()
        {
        }

        private void OnRepliesLinkChanging(Uri value)
        {
        }

        public ActivityUser ActivityActor
        {
            get
            {
                return this._ActivityActor;
            }
            set
            {
                this.OnActivityActorChanging(value);
                this._ActivityActor = value;
                this.OnActivityActorChanged();
                this.OnPropertyChanged("ActivityActor");
            }
        }

        public string ActivityVerb
        {
            get
            {
                return this._ActivityVerb;
            }
            set
            {
                this.OnActivityVerbChanging(value);
                this._ActivityVerb = value;
                this.OnActivityVerbChanged();
                this.OnPropertyChanged("ActivityVerb");
            }
        }

        public string ApplicationId
        {
            get
            {
                return this._ApplicationId;
            }
             set
            {
                this.OnApplicationIdChanging(value);
                this._ApplicationId = value;
                this.OnApplicationIdChanged();
                this.OnPropertyChanged("ApplicationId");
            }
        }

        public string ApplicationLink
        {
            get
            {
                return this._ApplicationLink;
            }
            set
            {
                this.OnApplicationLinkChanging(value);
                this._ApplicationLink = value;
                this.OnApplicationLinkChanged();
                this.OnPropertyChanged("ApplicationLink");
            }
        }

        public string ApplicationName
        {
            get
            {
                return this._ApplicationName;
            }
             set
            {
                this.OnApplicationNameChanging(value);
                this._ApplicationName = value;
                this.OnApplicationNameChanged();
                this.OnPropertyChanged("ApplicationName");
            }
        }

        public string HtmlContent
        {
            get
            {
                return this._HtmlContent;
            }
             set
            {
                this.OnHtmlContentChanging(value);
                this._HtmlContent = value;
                this.OnHtmlContentChanged();
                this.OnPropertyChanged("HtmlContent");
            }
        }

        public string HtmlSummary
        {
            get
            {
                return this._HtmlSummary;
            }
             set
            {
                this.OnHtmlSummaryChanging(value);
                this._HtmlSummary = value;
                this.OnHtmlSummaryChanged();
                this.OnPropertyChanged("HtmlSummary");
            }
        }

        public DateTime LastReplyTime
        {
            get
            {
                return this._LastReplyTime;
            }
             set
            {
                this.OnLastReplyTimeChanging(value);
                this._LastReplyTime = value;
                this.OnLastReplyTimeChanged();
                this.OnPropertyChanged("LastReplyTime");
            }
        }

        public LiveDataServiceCollection<AddCommentActivity> replies
        {
            get
            {
                if (this.trackedReplies == null)
                {
                    this.trackedReplies = new LiveDataServiceCollection<AddCommentActivity>(this);
                }
                return this.trackedReplies;
            }
        }

        public int RepliesCount
        {
            get
            {
                return this._RepliesCount;
            }
             set
            {
                this.OnRepliesCountChanging(value);
                this._RepliesCount = value;
                this.OnRepliesCountChanged();
                this.OnPropertyChanged("RepliesCount");
            }
        }
    }
}

