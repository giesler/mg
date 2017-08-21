namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id"), EntitySet("Files")]
    public class Document : FileEntry
    {
        private FileMediaContent _MediaContent;
        private bool _MediaContentInitialized;

        public FileMediaContent MediaContent
        {
            get
            {
                if ((this._MediaContent == null) && !this._MediaContentInitialized)
                {
                    this._MediaContent = new FileMediaContent();
                    this._MediaContentInitialized = true;
                }
                return this._MediaContent;
            }
             set
            {
                this._MediaContent = value;
                this._MediaContentInitialized = true;
            }
        }
    }
}

