namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    //[EntitySet("Albums"), EntityPropertyMapping("AlbumCover", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/AlbumCover"), EntityPropertyMapping("SlideShow", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/SlideShow"), DataServiceKey("Id")]
    [EntityPropertyMappingEx("AlbumCover", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/AlbumCover"), 
    EntityPropertyMappingEx("SlideShow", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/SlideShow")]
    [EntitySet("Albums"), 
    //EntityPropertyMapping("AlbumCover", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    //EntityPropertyMapping("SlideShow", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    DataServiceKey("Id")]
    public class Album : TopLevelFolder
    {
        private Uri _AlbumCover;
        private Uri _SlideShow;

        public Uri AlbumCover
        {
            get
            {
                return this._AlbumCover;
            }
            internal set
            {
                this._AlbumCover = value;
                this.OnPropertyChanged("AlbumCover");
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

