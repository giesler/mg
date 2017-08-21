namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    //[EntityPropertyMapping("Browse", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Browse"), EntityPropertyMapping("SlideShow", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/SlideShow"), EntitySet("Files"), DataServiceKey("Id")]
    [EntityPropertyMappingEx("Browse", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Browse"), 
    EntityPropertyMappingEx("SlideShow", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/SlideShow")]
    [
    //EntityPropertyMapping("Browse", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    //EntityPropertyMapping("SlideShow", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    EntitySet("Files"), DataServiceKey("Id")]
    public class FolderEntry : FileEntry
    {
        private Uri _Browse;
        private LiveDataServiceCollection<FileEntry> _Files;
        private Uri _SlideShow;

        public Uri Browse
        {
            get
            {
                return this._Browse;
            }
            internal set
            {
                this._Browse = value;
            }
        }

        public LiveDataServiceCollection<FileEntry> Files
        {
            get
            {
                if (this._Files == null)
                {
                    this._Files = new LiveDataServiceCollection<FileEntry>(this);
                }
                return this._Files;
            }
        }

        public Uri SlideShow
        {
            get
            {
                return this._SlideShow;
            }
            internal set
            {
                this._SlideShow = value;
            }
        }
    }
}

