namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [EntitySet("Comments"), DataServiceKey("Id")]
    public class Comment : LiveResource
    {
        private string _CommentText;

        public string CommentText
        {
            get
            {
                return this._CommentText;
            }
            set
            {
                this._CommentText = value;
            }
        }
    }
}

