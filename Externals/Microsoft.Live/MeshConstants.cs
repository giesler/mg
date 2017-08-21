using System;
using System.Xml;

internal static class MeshConstants
{
    internal static class ActivityBaseSchemas
    {
        internal const string Article = "http://activitystrea.ms/schema/1.0/article";
        internal const string AvatarRelType = "avatar";
        internal const string Bookmark = "http://activitystrea.ms/schema/1.0/bookmark";
        internal const string Comment = "http://activitystrea.ms/schema/1.0/comment";
        internal const string Favorite = "http://activitystrea.ms/schema/1.0/favorite";
        internal const string File = "http://activitystrea.ms/schema/1.0/file";
        internal const string Person = "http://activitystrea.ms/schema/1.0/person";
        internal const string Photo = "http://activitystrea.ms/schema/1.0/photo";
        internal const string PhotoAlbum = "http://activitystrea.ms/schema/1.0/photo-album";
        internal const string Play = "http://activitystrea.ms/schema/1.0/play";
        internal const string Post = "http://activitystrea.ms/schema/1.0/post";
        internal const string Product = "http://activitystrea.ms/schema/1.0/product";
        internal const string Review = "http://activitystrea.ms/schema/1.0/review";
        internal const string Save = "http://activitystrea.ms/schema/1.0/save";
        internal const string Share = "http://activitystrea.ms/schema/1.0/share";
        internal const string Status = "http://activitystrea.ms/schema/1.0/status";
        internal const string Tag = "http://activitystrea.ms/schema/1.0/tag";
        internal const string Undefined = "http://activitystrea.ms/schema/1.0/undefined";
        internal const string Video = "http://activitystrea.ms/schema/1.0/video";
    }

    internal static class ActivityStream
    {
        internal const string Actor = "actor";
        internal const string Context = "context";
        internal const string Namespace = "http://activitystrea.ms/spec/1.0/";
        internal const string NamespacePrefix = "as";
        internal const string Object = "object";
        internal const string ObjectType = "object-type";
        internal const string Target = "target";
        internal const string Verb = "verb";
    }

    internal static class ApplicationOffers
    {
        internal const string ContactRead = "Contacts.Read";
        internal const string ContactWrite = "Contacts.Write";
        internal const string NewsFull = "News.Full";
        internal const string NewsRead = "News.Read";
        internal const string NewsWrite = "News.Write";
        internal const string ProfileRead = "Profiles.Read";
        internal const string SyncObjectFull = "SyncObject.Full";
        internal const string SyncObjectRead = "SyncObject.Read";
        internal const string SyncObjectWrite = "SyncObject.Write";
    }

    internal static class AssociationType
    {
        internal const string MediaResource = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/";
        internal const string Runtime = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/";
    }

    internal static class Atom
    {
        public const string ContentElementName = "content";
        public const string HrefAttributeName = "href";
        public const string IdElementName = "id";
        public const string LinkElementName = "link";
        public const string Namespace = "http://www.w3.org/2005/Atom";
        public const string NamespacePrefix = "atom";
        public const string RefAttributeName = "ref";
        public const string RelatedRelType = "related";
        public const string RelAttributeName = "rel";
        public const string SummaryElementName = "summary";
        public const string TitleElementName = "title";
        public const string TypeAttributeName = "type";
    }

    internal static class AtomMedia
    {
        public const string AlternateRelType = "alternate";
        public const string DescriptionElementName = "description";
        public const string EnclosureRelType = "enclosure";
        public const string HeightAttributeName = "height";
        public const string Namespace = "http://purl.org/syndication/atommedia";
        public const string NamespacePrefix = "media";
        public const string PreviewRelType = "preview";
        public const string WidthAttributeName = "width";
    }

    internal static class AtomThreading
    {
        internal static readonly XmlQualifiedName Count = new XmlQualifiedName("count", "http://purl.org/syndication/thread/1.0");
        internal const string InReplyToElement = "in-reply-to";
        internal const string Namespace = "http://purl.org/syndication/thread/1.0";
        internal const string NamespacePrefix = "thr";
        internal const string RepliesRelType = "replies";
        internal static readonly XmlQualifiedName Updated = new XmlQualifiedName("updated", "http://purl.org/syndication/thread/1.0");
    }

    internal static class AuthHeaderConstants
    {
        internal const string AllACLs = "*";
        internal const string ApplicationIdPrefix = "aid=";
        internal const char AuthorizationDelimiter = ';';
        internal const string CompactTokenPrefix = "t=";
        internal const string ConsentOfferPrefix = "co=";
        internal const string CreateACL = "C";
        internal const string DelegationTokenPrefix = "dt=";
        internal const string DeleteACL = "D";
        internal const string OAuthWrapTokenPrefix = "WRAP access_token=";
        internal const string PuidTokenPrefix = "puid=";
        internal const string ReadACL = "R";
        internal const string Separator = ",";
        internal const string SiteName = "site=";
        internal const string SLTQueryParameter = "slt";
        internal const string UpdateACL = "U";
    }

    internal static class Categories
    {
        internal const string AboutYouProfile = "AboutYouProfile";
        internal const string Album = "Album";
        internal const string Alert = "Alert";
        internal const string Application = "Application";
        internal const string ApplicationInstance = "ApplicationInstance";
        internal const string ApplicationInstanceDataEntry = "ApplicationInstanceDataEntry";
        internal const string ApplicationInstances = "ApplicationInstances";
        internal const string Applications = "Applications";
        internal const string Calendar = "Calendar";
        internal const string Calendars = "Calendars";
        internal const string Comment = "Comment";
        internal const string ConfigurationsFeed = "ConfigurationsFeed";
        internal const string ConfigurationsItem = "ConfigurationsItem";
        internal const string Contact = "Contact";
        internal const string ContactCategories = "ContactCategories";
        internal const string ContactCategory = "ContactCategory";
        internal const string ContactProfile = "ContactProfile";
        internal const string Contacts = "AllContacts";
        internal const string DataEntry = "SyncDataEntry";
        internal const string DataFeed = "DataEntries";
        internal const string DataFeedDescriptor = "SyncDataFeed";
        internal const string DataFeedDescriptors = "DataFeeds";
        internal const string DebugApplication = "DebugApplication";
        internal const string Device = "SyncDevice";
        internal const string DeviceMapping = "DeviceMapping";
        internal const string DeviceMappings = "DeviceMappings";
        internal const string Devices = "Devices";
        internal const string EducationProfile = "EducationProfile";
        internal const string Endpoint = "DeviceEndpoint";
        internal const string Endpoints = "Endpoints";
        internal const string Error = "Error";
        internal const string Event = "CalendarEvent";
        internal const string Events = "Events";
        internal const string Folder = "Folder";
        internal const string InstalledApplication = "InstalledApplication";
        internal const string InstalledApplications = "InstalledApplications";
        internal const string Invitation = "SyncInvitation";
        internal const string Invitations = "Invitations";
        internal const string MediaLinkEntry = "MediaLinkEntry";
        internal const string Member = "SyncMember";
        internal const string Members = "Members";
        internal const string NewsConfigurationItem = "NewsConfigurationItem";
        internal const string NewsFeed = "NewsFeed";
        internal const string NewsItem = "NewsItem";
        internal const string Notification = "Notification";
        internal const string NotificationQueue = "NotificationQueue";
        internal const string NotificationQueues = "NotificationQueues";
        internal const string Notifications = "Notifications";
        internal const string Photo = "Photo";
        internal const string Profile = "Profile";
        internal const string Profiles = "Profiles";
        internal const string ResourceFeed = "ResourceFeed";
        internal const string ResourceFeeds = "ResourceFeeds";
        internal const string SocialProfile = "SocialProfile";
        internal const string Status = "Status";
        internal const string StatusProfile = "StatusProfile";
        internal const string Subscription = "Subscription";
        internal const string Subscriptions = "Subscriptions";
        internal const string SyncObject = "SyncObject";
        internal const string SyncObjects = "SyncObjects";
        internal const string Tag = "Tag";
        internal const string Video = "Video";
        internal const string WorkProfile = "WorkProfile";
    }

    internal static class ContactFormat
    {
        internal const string Portable = "Portable";
    }

    internal static class Description
    {
        internal const string AuthenticationModel = "AuthenticationModel";
        internal const string AuthenticationOptions = "AuthenticationOption";
        internal const string ConcurrencyControl = "ConcurrencyControl";
        internal const string IsExtensible = "IsExtensible";
        internal const string IsSubscribable = "IsSubscribable";
        internal const string MediaType = "MediaType";
        internal const string MediaTypes = "MediaTypes";
        internal const string OfflineEditable = "IsOfflineEditable";
        internal const string QueryParameter = "QueryParameter";
        internal const string QueryParameters = "QueryParameters";
        internal const string Relationships = "Relationships";
        internal const string RequestHeader = "RequestHeader";
        internal const string ResourceCollectionDescription = "ResourceCollectionDescription";
        internal const string ResourceDescription = "ResourceDescription";
        internal const string ResourceDocumentDescription = "ResourceDocumentDescription";
        internal const string ResourceEntryDescription = "ResourceEntryDescription";
        internal const string ResourceOperation = "ResourceOperation";
        internal const string ResponseHeader = "ResponseHeader";
        internal const string StatusCode = "StatusCode";
        internal const string SupportedMediaTypes = "SupportedMediaTypes";
        internal const string SupportedMethods = "SupportedMethods";
        internal const string SupportsIncrementalReads = "SupportsIncrementalReads";
        internal const string SupportsPaginatedReads = "SupportsPaginatedReads";
        internal const string UriTemplate = "UriTemplate";
        internal const string Version = "4.0";

        internal static class Auth
        {
            internal const string Anonymous = "Anonymous";
            internal const string LiveIdDelegatedAuth = "WindowsLiveDelegated Auth";
            internal const string LiveIdUserAndDelegatedAuth = "WindowsLiveIDAuthentication,WindowsLiveDelegatedAuth";
            internal const string LiveIdUserAuth = "WindowsLiveIDAuthentication";
        }

        internal static class MethodProperties
        {
            internal const string CommonRequestHeaders = "CommonRequestHeaders";
            internal const string CommonResponseHeaders = "CommonResponseHeaders";
            internal const string ExpectedStatusCodes = "ExpectedStatusCodes";
            internal const string Name = "Name";
            internal const string ResourceSchema = "ResourceSchema";
        }

        internal static class QueryParam
        {
            internal const string CallbackDesc = "json-p callback name";
            internal const string ExpandDesc = "<name of the link relationship>";
            internal const string FilterDesc = "<where clause of the query>";
            internal const string FormatDesc = "json, pox, xml, atom, rss";
            internal const string Name = "Name";
            internal const string OrderByDesc = "<sort by a property name>";
            internal const string OrderByDescDesc = "<sort by descending on a property name>";
            internal const string PossibleValues = "PossibleValues";
            internal const string SkipDesc = "<number of pages of a collection you want to skip>";
            internal const string TopDesc = "<number of top pages you want to fetch>";
            internal const string TypeDesc = "portable";
        }
    }

    internal static class Enclosure
    {
        internal const string HashAlgorithm = "SHA256";
    }

    internal static class Extensions
    {
        internal const string SyncContactName = "SyncContactName";
        internal const string SyncContactVal = "SyncContactVal";
    }

    internal static class Formats
    {
        internal const string Atom10 = "Atom10";
        internal const string Rss20 = "Rss20";
    }

    internal static class HttpHeaders
    {
        internal const string Accept = "Accept";
        internal const string Allow = "Allow";
        internal const string AppId = "AppId";
        internal const string ApplicationId = "X-HTTP-Live-ApplicationId";
        internal const string Authorization = "Authorization";
        internal const string CacheControlHeaderName = "Cache-Control";
        internal const string Cid = "Cid";
        internal const string Cookie = "Cookie";
        internal const string ETag = "ETag";
        internal const string Expect = "Expect";
        internal const string IfMatch = "If-Match";
        internal const string IfModifiedSince = "If-Modified-Since";
        internal const string IfNoneMatch = "If-None-Match";
        internal const string MeshAcceptEncoding = "X-HTTP-Live-Accept-Encoding";
        internal const string MeshDefaultCategory = "X-HTTP-Live-Default-Category";
        internal const string MeshEnclosureDescriptorIds = "X-HTTP-Live-Enclosure-Descriptor-Ids";
        internal const string MeshIgnoreQueryExecutionFailure = "X-HTTP-Live-IgnoreQueryFailure";
        internal const string MeshIncludeEnclosureIds = "X-HTTP-Live-Include-Enclosure-Ids";
        internal const string MeshIncludePrivateMeshObjects = "X-HTTP-Live-Include-Private-MeshObjects";
        internal const string MeshInstanceMeshObject = "X-HTTP-Live-Instance-MeshObject";
        internal const string MeshInternalAPI = "X-HTTP-Live-InternalAPI";
        internal const string MeshPagedRead = "X-HTTP-Live-Paged-Read";
        internal const string MeshParentId = "X-HTTP-Live-Parent-Id";
        internal const string MeshQueryExecutionFailure = "X-HTTP-Live-Query-Execution-Failure";
        internal const string MeshQueryExecutionFailureDescription = "X-HTTP-Live-Query-Execution-Failure-Description";
        internal const string MeshReplaceHost = "X-HTTP-Live-ReplaceHost";
        internal const string MeshRequestId = "X-HTTP-Live-Request-Id";
        internal const string MeshSafeAgent = "X-HTTP-Live-Safe-Agent";
        internal const string MeshScriptContext = "X-HTTP-Live-ResourceScript-Context";
        internal const string MeshScriptTriggerFailure = "X-HTTP-Live-ResourceScript-Trigger-Execution-Failure";
        internal const string MeshScriptTriggerFailureDescription = "X-HTTP-Live-ResourceScript-Trigger-Execution-Failure-Description";
        internal const string MeshSignature = "X-HTTP-Live-Signature";
        internal const string MeshSyncOnlyClient = "X-HTTP-Live-SyncOnlyClient";
        internal const string MeshTooManyItems = "X-HTTP-Live-Too-Many-Items";
        internal const string MeshTotalItemCount = "X-HTTP-Live-Total-Item-Count";
        internal const string MeshTotalItemRemaining = "X-HTTP-Live-IncompleteEnclosureCount";
        internal const string MeshTotalItemUploaded = "X-HTTP-Live-CompleteEnclosureCount";
        internal const string MeshTotalSizeRemaining = "X-HTTP-Live-TotalSize-Remaining";
        internal const string MeshTotalSizeUploaded = "X-HTTP-Live-TotalSize-Uploaded";
        internal const string MeshUserCid = "X-HTTP-Live-UserCid";
        internal const string P3P = "P3P";
        internal const string ProvisionSync = "X-HTTP-Live-ProvisionSync";
        internal const string RuntimeHeaderPrefix = "X-HTTP-Live-";
        internal const string SetCookie = "Set-Cookie";
        internal const string StagedMediaResourceUri = "X-HTTP-Live-Staged-Media-Resource-Uri";
    }

    internal static class HttpHeaderStatusDescription
    {
        internal const string MeshTooManyItems = "Too many items found";
    }

    internal static class HttpMethods
    {
        public const string Delete = "DELETE";
        public const string Get = "GET";
        public const string Head = "HEAD";
        public const string Options = "OPTIONS";
        public const string Post = "POST";
        public const string Put = "PUT";
    }

    internal static class KnownSyncResourceTypes
    {
        internal const string ConfigurationsFeed = "LiveMeshConfiguration";
        internal const string FileDataEntry = "File";
        internal const string FolderDataEntry = "Folder";
        internal const string NewsFeed = "LiveMeshNews";
        internal const string NewsServiceInternal = "CloudData:NewsServiceInternal";
        internal const string RegistryAppDataEntry = "LiveRegistrySetting";
        internal const string RegistryAppDataFeed = "LiveRegistrySettings";
        internal const string RegistryAppFolder = "LiveAppSettings";
        internal const string SyncDataFeed = "LiveMeshFiles";
        internal const string SyncFolder = "LiveMeshFolder";
    }

    internal static class LinkTitles
    {
        internal const string ActionLink = "Action";
        internal const string Activities = "Activities";
        internal const string AlbumCover = "AlbumCover";
        internal const string Application = "Application";
        internal const string ApplicationInstance = "ApplicationInstance";
        internal const string ApplicationInstances = "ApplicationInstances";
        internal const string ApplicationLogo = "ApplicationLogo";
        internal const string ApplicationPackage = "ApplicationPackage";
        internal const string Applications = "Applications";
        internal const string AuthorProfile = "AuthorProfiles";
        internal const string Browse = "Browse";
        internal const string CalendarSubscription = "CalendarSubscription";
        internal const string CallbackHome = "CallbackHome";
        internal const string Comments = "Comments";
        internal const string Conflicts = "Conflicts";
        internal const string Contacts = "AllContacts";
        internal const string ContributorProfile = "ContributorProfiles";
        internal const string DataEntries = "DataEntries";
        internal const string DataEntry = "DataEntry";
        internal const string DataFeedDescriptor = "DataFeed";
        internal const string DataFeedDescriptors = "DataFeeds";
        internal const string Device = "Device";
        internal const string DeviceMapping = "DeviceMapping";
        internal const string DeviceMappings = "DeviceMappings";
        internal const string Devices = "Devices";
        internal const string Edit = "edit";
        internal const string EditMedia = "edit-media";
        internal const string Enclosure = "enclosure";
        internal const string Endpoint = "Endpoint";
        internal const string Endpoints = "Endpoints";
        internal const string Events = "Events";
        internal const string FeedSync = "Sync";
        internal const string Files = "Files";
        internal const string FolderMembers = "FolderMembers";
        internal const string IconLink = "IconImage";
        internal const string InstalledApplication = "InstalledApplication";
        internal const string InstalledApplications = "InstalledApplications";
        internal const string Invitation = "Invitation";
        internal const string Invitations = "Invitations";
        internal const string Media = "Media";
        internal const string MediaResources = "MediaResources";
        internal const string MediaStagingArea = "SyncContentStagingArea";
        internal const string Member = "Member";
        internal const string Members = "Members";
        internal const string NewsFeed = "NewsItems";
        internal const string NewsIcon = "NewsIcon";
        internal const string Next = "next";
        internal const string NotificationQueues = "NotificationQueues";
        internal const string Notifications = "Notifications";
        internal const string Parent = "Parent";
        internal const string PresenceIcon = "PresenceIcon";
        internal const string Previous = "previous";
        internal const string Profiles = "Profiles";
        internal const string Resource = "Resource";
        internal const string ResourceFeed = "ResourceFeed";
        internal const string ResourceFeeds = "ResourceFeeds";
        internal const string RootDocument = "RootDocument";
        internal const string Script = "Script";
        internal const string Self = "self";
        internal const string ServiceResources = "ServiceResources";
        internal const string SlideShow = "SlideShow";
        internal const string StagedMediaResource = "stagedMediaResource";
        internal const string Status = "Status";
        internal const string Subscription = "Subscriptions";
        internal const string Sync = "Sync";
        internal const string SyncObject = "SyncObject";
        internal const string SyncObjects = "SyncObjects";
        internal const string TaggerProfile = "TaggerProfile";
        internal const string Tags = "Tags";
        internal const string Thumbnail = "thumbnail";
        internal const string Thumbnail_104X104C = "Thumbnail_104X104C";
        internal const string Thumbnail_176X176 = "Thumbnail_176X176";
        internal const string Thumbnail_213X213C = "Thumbnail_213X213C";
        internal const string Thumbnail_320X320 = "Thumbnail_320X320";
        internal const string Thumbnail_48X48 = "Thumbnail_48X48";
        internal const string Thumbnail_48X48C = "Thumbnail_48X48C";
        internal const string Thumbnail_600X450 = "Thumbnail_600X450";
        internal const string Thumbnail_800X600 = "Thumbnail_800X600";
        internal const string Thumbnail_96X96 = "Thumbnail_96X96";
        internal const string ThumbnailImage = "ThumbnailImage";
        internal const string TokenBrowse = "TokenBrowse";
        internal const string TokenSlideShow = "TokenSlideShow";
        internal const string User = "NewsItem";
        internal const string Ux = "Ux";
        internal const string Webready = "webready";
        internal const string Winner = "Winner";
    }

    internal static class Locations
    {
        internal const string Runtime = "https://user-ctp.windows.net/";
        internal const string RuntimeKeyName = "ServiceUrl";
        internal const string RuntimeScript = "https://user-ctp.windows.net/V4.0/Script/";
        internal const string RuntimeScriptKeyName = "ScriptUrl";
    }

    internal static class MediaTypes
    {
        public const string ApplicationXml = "application/xml";
        public const string Atom = "application/atom+xml";
        public const string AtomActivity = "application/atom+xml;schema=activity";
        public const string ImageJpeg = "image/jpeg";
        public const string JavaScript = "text/javascript";
        public const string JSon = "application/json";
        public const string OctetStream = "application/octet-stream";
        public const string ResourceScriptPox = "application/resourceScript+xml";
        public const string ResourceScriptXaml = "application/xaml+xml";
        public const string Rss = "application/rss+xml";
        public const string SNAtomActivity = "application/atom+xml;schema=activity.ms";
        public const string TextHtml = "text/html";
        public const string TextPlain = "text/plain; charset=utf-8";
        public const string Xml = "text/xml";
    }

    internal static class MicrosoftLifestreamSchemas
    {
        internal const string Achievement = "http://api.live.net/schemas/1.0/achievement";
        internal const string Custom = "http://api.live.net/schemas/1.0/custom";
        internal const string Defeat = "http://api.live.net/schemas/1.0/defeated";
        internal const string Game = "http://api.live.net/schemas/1.0/game";
        internal const string HighScore = "http://api.live.net/schemas/1.0/highscore";
        internal const string List = "http://api.live.net/schemas/1.0/list";
        internal const string Rating = "http://api.live.net/schemas/1.0/rating";
    }

    internal static class NamespacePrefixes
    {
        internal const string Data = "d";
    }

    internal static class Properties
    {
        internal static readonly XmlQualifiedName Cid = new XmlQualifiedName("Cid", "http://schemas.microsoft.com/ado/2007/08/dataservices");
        internal const string CidFormat = "X16";
        internal static readonly XmlQualifiedName DeviceName = new XmlQualifiedName("DeviceName", "http://apis.live.net");
        internal static readonly XmlQualifiedName Expansion = new XmlQualifiedName("inline", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
        internal const string RagEndpointServiceType = "RemoteAccess";
    }

    internal static class QueryParameters
    {
        internal const string AnchorContact = "$anchor";
        internal const string count = "count";
        internal const string Deleted = "$deleted";
        internal const string Etag = "$etag";
        internal const string Expand = "$expand";
        internal const string Filter = "$filter";
        internal const string filterBy = "filterBy";
        internal const string filterOp = "filterOp";
        internal const string filterValue = "filterValue";
        internal const string Format = "$format";
        internal const string Forward = "$forward";
        internal const string OrderBy = "$orderBy";
        internal const string Page = "page";
        internal const string PageSize = "$pagesize";
        internal const string Pending = "Pending";
        internal const string Separator = "&";
        internal const string Skip = "$skip";
        internal const string sortBy = "sortBy";
        internal const string sortOrder = "sortOrder";
        internal const string startIndex = "startIndex";
        internal const string Top = "$top";
        internal const string Type = "$type";
        internal const string updatedSince = "updatedSince";
    }

    internal static class Reltypes
    {
        internal const string ActionLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ActionLink";
        internal const string Activities = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Activities";
        internal const string Activity = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Activity";
        internal const string AlbumCover = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/AlbumCover";
        internal const string Albums = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Albums";
        internal const string AllContacts = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/AllContacts";
        internal const string Application = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Application";
        internal const string ApplicationInstance = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationInstance";
        internal const string ApplicationInstances = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationInstances";
        internal const string ApplicationLogo = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationLogo";
        internal const string ApplicationOffer = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationOffer";
        internal const string ApplicationPackage = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationPackage";
        internal const string ApplicationResources = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ApplicationResources";
        internal const string Applications = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Applications";
        internal const string AuthorProfile = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/AuthorProfiles";
        internal const string Browse = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Browse";
        internal const string CallbackHome = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/CallbackHome";
        internal const string Comments = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Comments";
        internal const string Conflicts = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Conflicts";
        internal const string ContactCategory = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ContactCategory";
        internal const string Contacts = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Contacts";
        internal const string ContributorProfile = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ContributorProfiles";
        internal const string DataEntries = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Entries";
        internal const string DataEntry = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/DataEntry";
        internal const string DataFeedDescriptor = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/DataFeed";
        internal const string DataFeedDescriptors = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/DataFeeds";
        internal const string Device = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Device";
        internal const string DeviceMapping = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/DeviceMapping";
        internal const string DeviceMappings = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Mappings";
        internal const string Devices = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Devices";
        internal const string Edit = "edit";
        internal const string EditMedia = "edit-media";
        internal const string Enclosure = "enclosure";
        internal const string Endpoint = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Endpoint";
        internal const string Endpoints = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Endpoints";
        internal const string EventsLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Events";
        internal const string FeedSync = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Sync";
        internal const string FeedSyncEntry = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/SyncEntry";
        internal const string Files = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Files";
        internal const string Folders = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Folders";
        internal const string IconLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/IconLink";
        internal const string InstalledApplication = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/InstalledApplication";
        internal const string InstalledApplications = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/InstalledApplications";
        internal const string Media = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Media";
        internal const string MediaResources = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/MediaResources";
        internal const string MediaStagingArea = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/SyncContentStagingArea";
        internal const string Member = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Member";
        internal const string Members = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Members";
        internal const string MyActivities = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/MyActivities";
        internal const string News = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/News";
        internal const string NewsFeed = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/NewsItems";
        internal const string NewsIcon = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/NewsIcon";
        internal const string Next = "next";
        internal const string NotificationQueues = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/NotificationQueues";
        internal const string Notifications = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Notifications";
        internal const string Parent = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Parent";
        internal const string Previous = "previous";
        internal const string Profiles = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Profiles";
        internal const string Resource = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Resource";
        internal const string ResourceFeed = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ResourceFeed";
        internal const string ResourceFeeds = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ResourceFeeds";
        internal const string RootDocument = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/RootDocument";
        internal const string Script = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Script";
        internal const string Self = "self";
        internal const string ServiceResources = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/ServiceResources";
        internal const string SlideShow = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/SlideShow";
        internal const string StagedMediaResource = "stagedMediaResource";
        internal const string StatusLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/StatusLink";
        internal const string SubscribableUrlLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/CalendarSubscriptionLink";
        internal const string Subscription = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Subscriptions";
        internal const string Sync = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Sync";
        internal const string SyncObject = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/SyncObject";
        internal const string SyncObjects = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/SyncObjects";
        internal const string TaggerProfiles = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/TaggerProfiles";
        internal const string Tags = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/Tags";
        internal const string Thumbnail = "thumbnail";
        internal const string Thumbnail_104X104C = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_104X104C";
        internal const string Thumbnail_176X176 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_176X176";
        internal const string Thumbnail_213X213C = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_213X213C";
        internal const string Thumbnail_320X320 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_320X320";
        internal const string Thumbnail_48X48 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48";
        internal const string Thumbnail_48X48C = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48C";
        internal const string Thumbnail_600X450 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_600X450";
        internal const string Thumbnail_800X600 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_800X600";
        internal const string Thumbnail_96X96 = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_96X96";
        internal const string ThumbnailImageLink = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/ThumbnailImage";
        internal const string TokenBrowse = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/TokenBrowse";
        internal const string TokenSlideShow = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/TokenSlideShow";
        internal const string User = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/NewsItem";
        internal const string Ux = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux";
        internal const string Webready = "webready";
    }

    internal static class ResourceCollections
    {
        internal const string Albums = "Albums";
        internal const string Alerts = "Alerts";
        internal const string Calendar = "Calendar";
        internal const string Calendars = "Calendars";
        internal const string Contact = "Contact";
        internal const string ContactCategories = "Categories";
        internal const string ContactCategory = "ContactCategory";
        internal const string Contacts = "AllContacts";
        internal const string ContactsActivities = "ContactsActivities";
        internal const string DataFeedErrors = "DataFeedErrors";
        internal const string Devices = "Devices";
        internal const string Events = "Events";
        internal const string Folders = "Folders";
        internal const string Invitations = "Invitations";
        internal const string MyActivities = "MyActivities";
        internal const string NotificationQueues = "NotificationQueues";
        internal const string Notifications = "Notifications";
        internal const string People = "Contacts";
        internal const string Photos = "Photos";
        internal const string PhotosOf = "PhotosOf";
        internal const string Profiles = "Profiles";
        internal const string Script = "Script";
        internal const string Storage = "Profiles";
        internal const string StorageService = "StorageService";
        internal const string Subscription = "Subscription";
        internal const string Sync = "Sync";
        internal const string SyncObjects = "SyncObjects";
    }

    internal static class ResourceModel
    {
        internal const string ResourceCollection = "ResourceCollectionOf{0}";
        internal const string ResourceTriggers = "Triggers";

        internal static class Properties
        {
            internal const string PostCreateTrigger = "PostCreateTrigger";
            internal const string PostDeleteTrigger = "PostDeleteTrigger";
            internal const string PostUpdateTrigger = "PostUpdateTrigger";
            internal const string PreCreateTrigger = "PreCreateTrigger";
            internal const string PreDeleteTrigger = "PreDeleteTrigger";
            internal const string PreUpdateTrigger = "PreUpdateTrigger";
        }
    }

    internal static class ResourceNames
    {
        internal const string AboutYouProfile = "AboutYouProfile";
        internal const string Activity = "Activity";
        internal const string Album = "Album";
        internal const string AlbumFile = "AlbumFile";
        internal const string Application = "Application";
        internal const string ApplicationInstance = "ApplicationInstance";
        internal const string Calendar = "Calendar";
        internal const string Comment = "Comment";
        internal const string Contact = "Contact";
        internal const string ContactCategory = "ContactCategory";
        internal const string ContactProfile = "ContactProfile";
        internal const string DataEntry = "DataEntry";
        internal const string DataFeed = "DataFeed";
        internal const string Device = "Device";
        internal const string DeviceMapping = "DeviceMapping";
        internal const string DocumentFolder = "Folder";
        internal const string DocumentFolderFile = "FolderFile";
        internal const string EducationProfile = "EducationProfile";
        internal const string Endpoint = "Endpoint";
        internal const string Error = "Error";
        internal const string Event = "Event";
        internal const string FileEntry = "FileEntry";
        internal const string InstalledApplication = "InstalledApplication";
        internal const string Invitation = "Invitation";
        internal const string MediaResource = "MediaInfo";
        internal const string Member = "Member";
        internal const string News = "News";
        internal const string NotificationQueue = "NotificationQueue";
        internal const string PartnerPermissions = "PartnerPermissions";
        internal const string PassThrough = "PassThrough";
        internal const string Profile = "Profile";
        internal const string Script = "Script";
        internal const string ServiceDocument = "ServiceDocument";
        internal const string SocialProfile = "SocialProfile";
        internal const string Status = "Status";
        internal const string StatusProfile = "StatusProfile";
        internal const string Subscription = "Subscription";
        internal const string Sync = "Sync";
        internal const string SyncObject = "SyncObject";
        internal const string Tag = "Tag";
        internal const string WorkProfile = "WorkProfile";
        internal const string XDomainFile = "XDomainFile";
    }

    internal static class Schemas
    {
        internal const string DataServices = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        internal const string DataServicesMetadata = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        internal const string Sync = "http://apis.live.net";
    }

    internal static class Schemes
    {
        internal const string Categories = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";
        internal const string Contact = "http://apis.live.net/Contact/";
        internal const string DataEntry = "http://apis.live.net/SyncDataEntry";
        internal const string DataFeed = "http://apis.live.net/SyncDataFeed";
        internal const string MeshObjectId = "urn:live:";
        internal const string Sync = "http://apis.live.net/";
        internal const string SyncObject = "http://apis.live.net/SyncObject";
    }

    internal static class Scripting
    {
        internal static class Properties
        {
            internal const string Assignments = "Assignments";
            internal const string Authorization = "Authorization";
            internal const string Bindings = "Bindings";
            internal const string Body = "Body";
            internal const string Branches = "Branches";
            internal const string Children = "Children";
            internal const string Condition = "Condition";
            internal const string ConstantParameter = "ConstantParameter";
            internal const string ContinueOnError = "ContinueOnError";
            internal const string Count = "Count";
            internal const string Description = "Description";
            internal const string ElseBranch = "ElseBranch";
            internal const string Error = "Error";
            internal const string EvalExpression = "EvalExpression";
            internal const string Exception = "Exception";
            internal const string ExpressionBinding = "ExpressionBinding";
            internal const string ExternalMediaUrl = "ExternalMediaUrl";
            internal const string Fault = "Fault";
            internal const string FaultHandler = "FaultHandler";
            internal const string FaultType = "FaultType";
            internal const string Filter = "Filter";
            internal const string Handlers = "Handlers";
            internal const string IfBranch = "IfBranch";
            internal const string Invokee = "Invokee";
            internal const string Item = "Item";
            internal const string IterationData = "IterationData";
            internal const string Iterations = "Iterations";
            internal const string MediaResourceName = "MediaResourceName";
            internal const string MethodOverride = "MethodOverride";
            internal const string Name = "Name";
            internal const string ObjectParameter = "ObjectParameter";
            internal const string Parameter = "Parameter";
            internal const string ParameterBinding = "ParameterBinding";
            internal const string Parameters = "Parameters";
            internal const string Parent = "Parent";
            internal const string PropertyBinding = "PropertyBinding";
            internal const string ReadResponseHeadersOnly = "ReadResponseHeadersOnly";
            internal const string Request = "Request";
            internal const string RequestHeaders = "RequestHeaders";
            internal const string ResourceParameter = "ResourceParameter";
            internal const string Response = "Response";
            internal const string ResponseCookies = "ResponseCookies";
            internal const string ResponseHeaders = "ResponseHeaders";
            internal const string Result = "Result";
            internal const string RunAtServer = "RunAtServer";
            internal const string Script = "Script";
            internal const string ScriptContextId = "ScriptContextId";
            internal const string ScriptContextProperties = "ScriptContextProperties";
            internal const string ScriptFormat = "ScriptFormat";
            internal const string Source = "Source";
            internal const string SourceExpression = "SourceExpression";
            internal const string SourceParameter = "SourceParameter";
            internal const string SourceProperty = "SourceProperty";
            internal const string SourcePropertyName = "SourcePropertyName";
            internal const string SourceStatementName = "SourceStatementName";
            internal const string SourceType = "SourceType";
            internal const string StatementBinding = "StatementBinding";
            internal const string Statements = "Statements";
            internal const string StatusCode = "StatusCode";
            internal const string StatusDescription = "StatusDescription";
            internal const string Sync = "Sync";
            internal const string Target = "Target";
            internal const string TargetExpression = "TargetExpression";
            internal const string TargetProperty = "TargetProperty";
            internal const string TargetPropertyName = "TargetPropertyName";
            internal const string TargetStatementName = "TargetStatementName";
            internal const string TerminateCondition = "TerminateCondition";
            internal const string Type = "Type";
            internal const string Url = "Url";
            internal const string Value = "Value";
        }

        internal static class Runtime
        {
            internal const string DefaultEngine = "Microsoft.LiveFX.ResourceModel.Scripting.Runtime.DefaultResourceScriptEngine";
            internal const string ErrorsFound = "Validation Errors were found:\n";
            internal const string RuntimeNS = "Microsoft.LiveFX.ResourceModel.Scripting.Runtime";
        }

        internal static class Statements
        {
            internal const string Assign = "Assign";
            internal const string Bindings = "Bindings";
            internal const string CancellationHandler = "CancellationHandler";
            internal const string CompoundStatement = "CompoundStatement";
            internal const string Conditional = "Conditional";
            internal const string CreateMediaResource = "CreateMediaResource";
            internal const string CreateResource = "CreateResourceOf{0}";
            internal const string DeleteMediaResource = "DeleteMediaResource";
            internal const string DeleteResource = "DeleteResource";
            internal const string FaultHandler = "FaultHandler";
            internal const string FaultHandlers = "FaultHandlers";
            internal const string Interleave = "Interleave";
            internal const string InvokeResourceScript = "InvokeResourceScript";
            internal const string Loop = "Loop";
            internal const string ReadResource = "ReadResourceOf{0}";
            internal const string ReadResourceCollection = "ReadResourceCollectionOf{0}";
            internal const string ReadSync = "ReadSync";
            internal const string ResourceScript = "ResourceScript";
            internal const string ResourceScriptGenericName = "ResourceScriptOf{0}";
            internal const string Sequence = "Sequence";
            internal const string Statement = "Statement";
            internal const string StatementCollection = "StatementCollection";
            internal const string SynchronizeFeed = "SynchronizeFeed";
            internal const string Terminate = "Terminate";
            internal const string Throw = "Throw";
            internal const string Undo = "Undo";
            internal const string UndoHandler = "UndoHandler";
            internal const string UpdateMediaResource = "UpdateMediaResource";
            internal const string UpdateResource = "UpdateResourceOf{0}";
            internal const string WebOperation = "WebOperation";
            internal const string While = "While";
        }
    }

    internal static class ServiceNames
    {
        internal const string Accounts = "Accounts";
        internal const string Calendar = "Calendar";
        internal const string Documents = "Documents";
        internal const string Enclosures = "Enclosures";
        internal const string IDS = "IDS";
        internal const string Lifestream = "Activities";
        internal const string People = "Contacts";
        internal const string Photos = "Photos";
        internal const string Profiles = "Profiles";
        internal const string ResourceProvider = "ResourceProvider";
        internal const string ResourceScripts = "ResourceScripts";
        internal const string Root = "Root";
        internal const string ServiceStatus = "ServiceStatus";
        internal const string Sync = "Sync";
    }

    internal static class SNLifeStreams
    {
        internal const string ActivityIdElementName = "activityId";
        internal const string AppIdElementName = "appId";
        internal const string AppNameElementName = "appName";
        internal const string CommentIdElementName = "commentId";
        internal const string Defeat = "http://activitystrea.ms/schema/1.0/defeated";
        internal const string HighScore = "http://activitystrea.ms/schema/1.0/highscore";
        internal const string IdElementName = "id";
        internal const string LabelAttributeName = "label";
        internal const string Namespace = "http://api.live.com/schemas";
        internal const string Person = "http://api.live.net/schemas/1.0/person";
        internal const string Rate = "http://api.live.net/schemas/1.0/rating";
        internal const string RateVerb = "http://activitystrea.ms/schema/1.0/rate";
        internal const string RatingElementName = "rating";
    }

    internal static class Sse
    {
        public const string SseNamepacePrefix = "sx";
        public const string SseNamespace = "http://feedsync.org/2007/feedsync";
        public const string SyncElementName = "sync";
    }

    internal static class Terms
    {
        internal const string SyncObject = "SyncObject";
    }

    internal static class ThrottleConstants
    {
        public const int PageSize = 100;
        public static readonly string PrincipalThrottleType = "Pr";
    }

    internal static class Workspaces
    {
        internal const string Calendar = "Calendar";
        internal const string CurrentVersion = "v4.0";
        internal const string Documents = "Documents";
        internal const string People = "Contacts";
        internal const string Photos = "Photos";
        internal const string SupportedVersions = "SupportedVersions";
        internal const string Sync = "Sync";
        internal const string WLS = "Profiles";
    }

    internal static class XmlNames
    {
        internal const string AboutYouProfile = "AboutYouProfile";
        internal const string AboutYouProfileContent = "AboutYouProfileContent";
        internal const string AcceptInvitationLink = "AcceptInvitationLink";
        internal const string ActionLink = "ActionLink";
        internal const string ActivitiesLink = "ActivitiesLink";
        internal const string Activity = "Activity";
        internal const string ActivityAchievement = "ActivityAchievement";
        internal const string ActivityActor = "ActivityActor";
        internal const string ActivityArticle = "ActivityArticle";
        internal const string ActivityBlogEntry = "ActivityBlogEntry";
        internal const string ActivityBookmark = "ActivityBookmark";
        internal const string ActivityComment = "ActivityComment";
        internal const string ActivityContext = "ActivityContext";
        internal const string ActivityCustom = "ActivityCustom";
        internal const string ActivityFile = "ActivityFile";
        internal const string ActivityGame = "ActivityGame";
        internal const string ActivityHighScore = "ActivityHighScore";
        internal const string ActivityLink = "ActivityLink";
        internal const string ActivityList = "ActivityList";
        internal const string ActivityObject = "ActivityObject";
        internal const string ActivityObjects = "ActivityObjects";
        internal const string ActivityObjectType = "ActivityObjectType";
        internal const string ActivityPhoto = "ActivityPhoto";
        internal const string ActivityPhotoAlbum = "ActivityPhotoAlbum";
        internal const string ActivityProduct = "ActivityProduct";
        internal const string ActivityRating = "ActivityRating";
        internal const string ActivityResourceContent = "ActivityResourceContent";
        internal const string ActivityReview = "ActivityReview";
        internal const string ActivityStatus = "ActivityStatus";
        internal const string ActivityTarget = "ActivityTarget";
        internal const string ActivityTime = "ActivityTime";
        internal const string ActivityUser = "ActivityUser";
        internal const string ActivityVerb = "ActivityVerb";
        internal const string ActivityVideo = "ActivityVideo";
        internal const string AddArticleActivity = "AddArticleActivity";
        internal const string AddBookmarkActivity = "AddBookmarkActivity";
        internal const string AddCommentActivity = "AddCommentActivity";
        internal const string AddPhotoActivity = "AddPhotoActivity";
        internal const string AddProductActivity = "AddProductActivity";
        internal const string Address = "Address";
        internal const string AddReviewActivity = "AddReviewActivity";
        internal const string AddStatusActivity = "AddStatusActivity";
        internal const string AddVideoActivity = "AddVideoActivity";
        internal const string AlbumCoverLink = "AlbumCoverLink";
        internal const string AlbumResource = "Album";
        internal const string AlbumResourceContent = "AlbumContent";
        internal const string AlbumsExpansion = "AlbumsExpansion";
        internal const string AlbumsLink = "AlbumsLink";
        internal const string AlertsLink = "AlertsLink";
        internal const string AlertsResource = "AlertsResource";
        internal const string AllowAnyoneToAccept = "AllowAnyoneToAccept";
        internal const string AlternateLink = "AlternateLink";
        internal const string anniversary = "anniversary";
        internal const string Anniversary = "Anniversary";
        internal const string AppletLink = "AppletLink";
        internal const string ApplicationClaimTicket = "ApplicationClaimTicket";
        internal const string ApplicationId = "ApplicationId";
        internal const string ApplicationInstanceLink = "ApplicationInstanceLink";
        internal const string ApplicationInstanceMappingUrls = "ApplicationInstanceMappingUrls";
        internal const string ApplicationInstanceResource = "ApplicationInstance";
        internal const string ApplicationInstanceResourceContent = "ApplicationInstanceResourceContent";
        internal const string ApplicationInstancesLink = "ApplicationInstancesLink";
        internal const string ApplicationLink = "ApplicationLink";
        internal const string ApplicationLogoLink = "ApplicationLogoLink";
        internal const string ApplicationManifest = "ApplicationManifest";
        internal const string ApplicationMeshObjectId = "ApplicationMeshObjectId";
        internal const string ApplicationName = "ApplicationName";
        internal const string ApplicationPackageType = "ApplicationPackageType";
        internal const string ApplicationReleaseStatus = "ApplicationReleaseStatus";
        internal const string ApplicationResource = "Application";
        internal const string ApplicationResourceContent = "ApplicationContent";
        internal const string ApplicationResourcesLink = "ApplicationResourcesLink";
        internal const string ApplicationsLink = "ApplicationsLink";
        internal const string ApplicationType = "ApplicationType";
        internal const string Approved = "Approved";
        internal const string AttributeExtensions = "AttributeExtensions";
        internal const string Attributes = "Attributes";
        internal const string Author = "Author";
        internal const string AuthorProfileExpansion = "AuthorProfilesExpansion";
        internal const string AuthorProfileLink = "AuthorProfilesLink";
        internal const string Availability = "Availability";
        internal const string AvatarLink = "AvatarLink";
        internal const string BaseUri = "BaseUri";
        internal const string birthday = "birthday";
        internal const string Birthday = "Birthday";
        internal const string BirthDay = "BirthDay";
        internal const string BirthMonth = "BirthMonth";
        internal const string BirthYear = "BirthYear";
        internal const string BitRate = "BitRate";
        internal const string Blog = "Blog";
        internal const string Body = "Body";
        internal const string BrowseLink = "BrowseLink";
        internal const string Business = "Business";
        internal const string CachedApplicationContent = "CachedApplicationContent";
        internal const string CalendarEventResource = "Event";
        internal const string CalendarExpansion = "CalendarExpansion";
        internal const string CalendarLink = "CalendarLink";
        internal const string CalendarResource = "Calendar";
        internal const string CalendarServiceResource = "CalendarServiceDocument";
        internal const string CalendarsExpansion = "CalendarsExpansion";
        internal const string CalendarsLink = "CalendarsLink";
        internal const string CalendarTimeZone = "CalendarTimeZone";
        internal const string CalendarTimeZoneComponent = "CalendarTimeZoneComponent";
        internal const string CalendarTimeZoneDaylightComponentName = "Daylight";
        internal const string CalendarTimeZoneStandardComponentName = "Standard";
        internal const string CallbackLink = "CallbackLink";
        internal const string CanCreateTags = "CanCreateTags";
        internal const string Categories = "Categories";
        internal const string CategoriesExpansion = "CategoriesExpansion";
        internal const string CategoriesLink = "CategoriesLink";
        internal const string Category = "Category";
        internal const string CategoryContactsExpansion = "ContactsExpansion";
        internal const string CategoryContactsLink = "ContactsLink";
        internal const string CategoryInfo = "CategoryInfo";
        internal const string CategoryLink = "CategoryLink";
        internal const string Cid = "Cid";
        internal const string City = "City";
        internal const string CloudSync = "CloudSync";
        internal const string CoalesceCount = "CoalesceCount";
        internal const string Code = "Code";
        internal const string CollectionInfo = "CollectionInfo";
        internal const string CollectionLink = "CollectionLink";
        internal const string Collections = "Collections";
        internal const string CommentCount = "CommentCount";
        internal const string CommentResource = "CommentResource";
        internal const string CommentResourceContent = "CommentContent";
        internal const string CommentsEnabled = "CommentsEnabled";
        internal const string CommentsExpansion = "CommentsExpansion";
        internal const string CommentsLink = "CommentsLink";
        internal const string CommentText = "CommentText";
        internal const string Company = "Company";
        internal const string CompanyName = "CompanyName";
        internal const string ConflictPath = "ConflictPath";
        internal const string ConflictResolver = "ConflictResolver";
        internal const string ConflictsExpansion = "ConflictsExpansion";
        internal const string ConflictsLink = "ConflictsLink";
        internal const string ContactCategoryResource = "ContactCategoryResource";
        internal const string ContactCategoryResourceContent = "ContactCategoryResourceContent";
        internal const string ContactEmail = "ContactEmail";
        internal const string ContactEmailType = "ContactEmailType";
        internal const string ContactId = "ContactId";
        internal const string ContactIm = "ContactIm";
        internal const string ContactImType = "ContactImType";
        internal const string ContactPhone = "ContactPhone";
        internal const string ContactPhoneType = "ContactPhoneType";
        internal const string ContactProfile = "ContactProfile";
        internal const string ContactProfileContent = "ContactProfileContent";
        internal const string ContactResource = "Contact";
        internal const string ContactResourceContent = "ContactContent";
        internal const string ContactsActivitiesExpansion = "ContactsActivitiesExpansion";
        internal const string ContactsActivitiesLink = "ContactsActivitiesLink";
        internal const string ContactsExpansion = "AllContactsExpansion";
        internal const string ContactsLink = "AllContactsLink";
        internal const string ContactTag = "ContactTag";
        internal const string ContactTagType = "ContactTagType";
        internal const string ContactType = "ContactType";
        internal const string ContactUrl = "ContactUrl";
        internal const string ContactUrlType = "ContactUrlType";
        internal const string Content = "Content";
        internal const string Contributor = "Contributor";
        internal const string ContributorProfileExpansion = "ContributorProfilesExpansion";
        internal const string ContributorProfileLink = "ContributorProfilesLink";
        internal const string Coordinates = "Coordinates";
        internal const string Copyright = "Copyright";
        internal const string country = "country";
        internal const string CountryRegion = "CountryRegion";
        internal const string CreatorId = "CreatorId";
        internal const string CurrentVersion = "CurrentVersion";
        internal const string CustomActivity = "CustomActivity";
        internal const string CustomActivityResourceContent = "CustomActivityResourceContent";
        internal const string CustomActivityVerb = "CustomActivityVerb";
        internal const string DataEntriesExpansion = "EntriesExpansion";
        internal const string DataEntriesLink = "EntriesLink";
        internal const string DataEntryLocalState = "DataEntryLocalState";
        internal const string DataEntryResource = "DataEntry";
        internal const string DataEntryResourceContent = "DataEntryContent";
        internal const string DataFeedLocalState = "DataFeedLocalState";
        internal const string DataFeedMapping = "DataFeedMapping";
        internal const string DataFeedMappings = "DataFeedMappings";
        internal const string DataFeedResource = "DataFeed";
        internal const string DataFeedResourceContent = "DataFeedContent";
        internal const string DataFeedsExpansion = "DataFeedsExpansion";
        internal const string DataFeedsLink = "DataFeedsLink";
        internal const string DeclineInvitationLink = "DeclineInvitationLink";
        internal const string DefeatUserActivity = "DefeatUserActivity";
        internal const string Degree = "Degree";
        internal const string DelegationAppId = "DelegationAppId";
        internal const string Description = "Description";
        internal const string Details = "Details";
        internal const string DeveloperCid = "DeveloperCid";
        internal const string DeveloperEmail = "DeveloperEmail";
        internal const string DeviceExpansion = "DeviceExpansion";
        internal const string DeviceLink = "DeviceLink";
        internal const string DeviceLogicalId = "DeviceLogicalId";
        internal const string DeviceMappingResource = "DeviceMapping";
        internal const string DeviceMappingResourceContent = "DeviceMappingContent";
        internal const string DeviceMappingsExpansion = "MappingsExpansion";
        internal const string DeviceMappingsLink = "MappingsLink";
        internal const string DeviceName = "DeviceName";
        internal const string DevicesExpansion = "DevicesExpansion";
        internal const string DevicesLink = "DevicesLink";
        internal const string displayName = "displayName";
        internal const string DisplayName = "DisplayName";
        internal const string DisplayVersion = "DisplayVersion";
        internal const string DocumentsExpansion = "DocumentsExpansion";
        internal const string DocumentsLink = "DocumentsLink";
        internal const string Domain = "Domain";
        internal const string DomainId = "DomainId";
        internal const string DownloadInProgress = "DownloadInProgress";
        internal const string DownloadState = "DownloadState";
        internal const string DtStart = "DtStart";
        internal const string Duration = "Duration";
        internal const string EditMediaLink = "EditMediaLink";
        internal const string EducationProfile = "EducationProfile";
        internal const string EducationProfileContent = "EducationProfileContent";
        internal const string Element = "element";
        internal const string Email = "Email";
        internal const string emails = "emails";
        internal const string Emails = "Emails";
        internal const string EmailText = "EmailText";
        internal const string EnclosureLink = "EnclosureLink";
        internal const string EndpointResource = "EndpointResource";
        internal const string EndpointResourceContent = "EndpointResourceContent";
        internal const string EndpointsExpansion = "EndpointsExpansion";
        internal const string EndpointsLink = "EndpointsLink";
        internal const string EndTimeZoneOffset = "EndTimeZoneOffset";
        internal const string entries = "entries";
        internal const string Entries = "Entries";
        internal const string Error = "Error";
        internal const string ErrorCode = "code";
        internal const string ErrorContent = "error";
        internal const string ErrorMessage = "message";
        internal const string ErrorResource = "Error";
        internal const string ErrorResourceContent = "error";
        internal const string Errors = "Errors";
        internal const string etag = "etag";
        internal const string ETag = "ETag";
        internal const string EventsExpansion = "EventsExpansion";
        internal const string EventsLink = "EventsLink";
        internal const string EventType = "EventType";
        internal const string ExecutingRequest = "ExecutingRequest";
        internal const string ExpandedLink = "ExpandedLink";
        internal const string ExpandedLocalPath = "ExpandedLocalPath";
        internal const string Expansion = "Expansion";
        internal const string ExpansionInfo = "ExpansionInfo";
        internal const string Expansions = "Expansions";
        internal const string Expiration = "Expiration";
        internal const string ExpirationDuration = "ExpirationDuration";
        internal const string Expires = "Expires";
        internal const string FailedRequest = "FailedRequest";
        internal const string FamilyName = "FamilyName";
        internal const string Fashion = "Fashion";
        internal const string FashionTaste = "FashionTaste";
        internal const string FashionType = "FashionType";
        internal const string FavoriteQuote = "FavoriteQuote";
        internal const string FederatedStorage = "FederatedStorage";
        internal const string FederatedStorageLinks = "FederatedStorageLinks";
        internal const string FeedUrl = "FeedUrl";
        internal const string FileContent = "FileContent";
        internal const string FileContentLink = "FileContentLink";
        internal const string FileMediaContent = "FileMediaContent";
        internal const string FileResource = "File";
        internal const string FilesExpansion = "FilesExpansion";
        internal const string FileSize = "FileSize";
        internal const string FilesLink = "FilesLink";
        internal const string FilesToDownload = "FilesToDownload";
        internal const string FilesTotal = "FilesTotal";
        internal const string filtered = "filtered";
        internal const string FirstName = "FirstName";
        internal const string FolderResource = "Folder";
        internal const string FoldersExpansion = "FoldersExpansion";
        internal const string FoldersLink = "FoldersLink";
        internal const string formatted = "formatted";
        internal const string Formatted = "Formatted";
        internal const string FormattedName = "FormattedName";
        internal const string FreeText = "FreeText";
        internal const string gender = "gender";
        internal const string Gender = "Gender";
        internal const string GenderType = "GenderType";
        internal const string GeneralProfileContent = "GeneralProfileContent";
        internal const string GeoLocation = "GeoLocation";
        internal const string GivenName = "GivenName";
        internal const string GraduationYear = "GraduationYear";
        internal const string GreekOrganization = "GreekOrganization";
        internal const string Hash = "Hash";
        internal const string HashAlgorithm = "HashAlgorithm";
        internal const string Height = "Height";
        internal const string Home = "Home";
        internal const string HomeAddress = "HomeAddress";
        internal const string HomeAddress2 = "HomeAddress2";
        internal const string HomeFax = "HomeFax";
        internal const string HomePhone = "HomePhone";
        internal const string Hometown = "Hometown";
        internal const string HonorificPrefix = "HonorificPrefix";
        internal const string HonorificSuffix = "HonorificSuffix";
        internal const string HtmlContent = "HtmlContent";
        internal const string HtmlSummary = "HtmlSummary";
        internal const string Humor = "Humor";
        internal const string HumorType = "HumorType";
        internal const string IconLink = "IconLink";
        internal const string IconUrl = "IconUrl";
        internal const string id = "id";
        internal const string Id = "Id";
        internal const string Identity = "Identity";
        internal const string IdentityPartitionKey = "IdentityPartitionKey";
        internal const string Ims = "Ims";
        internal const string Infoset = "Infoset";
        internal const string Inline = "inline";
        internal const string InReplyTo = "InReplyTo";
        internal const string InstalledApplicationId = "InstalledApplicationId";
        internal const string InstalledApplicationLink = "InstalledApplicationLink";
        internal const string InstalledApplicationMetadata = "InstalledApplicationMetadata";
        internal const string InstalledApplicationResource = "InstalledApplication";
        internal const string InstalledApplicationResourceContent = "InstalledApplicationResourceContent";
        internal const string InstalledApplicationsLink = "InstalledApplicationsLink";
        internal const string InstalledVersion = "InstalledVersion";
        internal const string InstallReturnLink = "InstallReturnLink";
        internal const string InstancesIds = "InstancesIds";
        internal const string InterestedIn = "InterestedIn";
        internal const string Interests = "Interests";
        internal const string Invitation = "Invitation";
        internal const string InvitationEmailAddress = "InvitationEmailAddress";
        internal const string InvitationLink = "InvitationLink";
        internal const string InvitationResourceContent = "InvitationContent";
        internal const string InvitationsExpansion = "InvitationsExpansion";
        internal const string InvitationsLink = "InvitationsLink";
        internal const string InvitedBy = "InvitedBy";
        internal const string IsAllDayEvent = "IsAllDayEvent";
        internal const string IsAutoCloudSynced = "IsAutoCloudSynced";
        internal const string IsCloudSynced = "IsCloudSynced";
        internal const string IsDefaultCalendar = "IsDefaultCalendar";
        internal const string IsDeleted = "IsDeleted";
        internal const string IsFriend = "IsFriend";
        internal const string IsGhosted = "IsGhosted";
        internal const string IsIMEnabled = "IsIMEnabled";
        internal const string IsLocal = "IsLocal";
        internal const string IsMeetingRequest = "IsMeetingRequest";
        internal const string IsOnline = "IsOnline";
        internal const string IsOrganizer = "IsOrganizer";
        internal const string IsOwner = "IsOwner";
        internal const string IsPending = "IsPending";
        internal const string isprimary = "isprimary";
        internal const string IsPrimary = "IsPrimary";
        internal const string IsRemoteConnectionActive = "IsRemoteConnectionActive";
        internal const string IsShared = "IsShared";
        internal const string IsUploaded = "IsUploaded";
        internal const string IsXmlContent = "IsXmlContent";
        internal const string itemsPerPage = "itemsPerPage";
        internal const string JobTitle = "JobTitle";
        internal const string Key = "Key";
        internal const string Kind = "Kind";
        internal const string Label = "Label";
        internal const string Landline = "Landline";
        internal const string LastChanged = "LastChanged";
        internal const string LastName = "LastName";
        internal const string LastReplyTime = "LastReplyTime";
        internal const string LastSyncTime = "LastSyncTime";
        internal const string Latency = "Latency";
        internal const string Latitude = "Latitude";
        internal const string Length = "Length";
        internal const string LifestreamExpansion = "ActivitiesExpansion";
        internal const string LifestreamLink = "ActivitiesLink";
        internal const string Lifestreams = "Lifestreams";
        internal const string Link = "Link";
        internal const string Links = "Links";
        internal const string Locale = "Locale";
        internal const string locality = "locality";
        internal const string LocalPath = "LocalPath";
        internal const string LocalRelativePath = "LocalRelativePath";
        internal const string LocalSpecialFolder = "LocalSpecialFolder";
        internal const string LocalState = "LocalState";
        internal const string location = "location";
        internal const string Location = "Location";
        internal const string locations = "locations";
        internal const string Locations = "Locations";
        internal const string LogoLink = "LogoLink";
        internal const string LogoName = "LogoName";
        internal const string Longitude = "Longitude";
        internal const string Major = "Major";
        internal const string Manifest = "Manifest";
        internal const string ManifestLink = "ManifestLink";
        internal const string ManifestVersion = "ManifestVersion";
        internal const string MarkFavoriteActivity = "MarkFavoriteActivity";
        internal const string MaxAge = "MaxAge";
        internal const string MaxHeight = "MaxHeight";
        internal const string MaxQuota = "MaxQuota";
        internal const string MaxWidth = "MaxWidth";
        internal const string MediaContent = "MediaContent";
        internal const string MediaInfo = "MediaInfo";
        internal const string MediaLink = "MediaLink";
        internal const string MediaThumbnail = "MediaThumbnail";
        internal const string MediaType = "MediaType";
        internal const string MemberExpansion = "MemberExpansion";
        internal const string MemberInvitation = "MemberInvitation";
        internal const string MemberLink = "MemberLink";
        internal const string MemberResource = "Member";
        internal const string MemberResourceContent = "MemberContent";
        internal const string MembersExpansion = "MembersExpansion";
        internal const string MembersLink = "MembersLink";
        internal const string MeshDeviceResource = "Device";
        internal const string MeshDeviceResourceContent = "DeviceContent";
        internal const string Message = "Message";
        internal const string MetadataId = "MetadataId";
        internal const string MiddleName = "MiddleName";
        internal const string MimeType = "MimeType";
        internal const string Mobile = "Mobile";
        internal const string MobilePhone = "MobilePhone";
        internal const string MoreAboutMe = "MoreAboutMe";
        internal const string MultiInstance = "MultiInstance";
        internal const string MusicTaste = "MusicTaste";
        internal const string MyActivitiesExpansion = "MyActivitiesExpansion";
        internal const string MyActivitiesLink = "MyActivitiesLink";
        internal const string name = "name";
        internal const string Name = "Name";
        internal const string NewsContexts = "Contexts";
        internal const string NewsFeedLink = "NewsFeedLink";
        internal const string NewsItemContext = "NewsItemContext";
        internal const string NewsItemContextKind = "NewsItemContextKind";
        internal const string NewsItemResource = "NewsItem";
        internal const string NewsItemResourceContent = "NewsItemContent";
        internal const string NextLink = "NextLink";
        internal const string nickname = "nickname";
        internal const string Nickname = "Nickname";
        internal const string NonXmlContent = "NonXmlContent";
        internal const string Notification = "Notification";
        internal const string NotificationQueue = "NotificationQueue";
        internal const string NotificationQueueLink = "NotificationQueueLink";
        internal const string NotificationQueueResourceContent = "NotificationQueueContent";
        internal const string NotificationQueuesLink = "NotificationQueuesLink";
        internal const string NotificationResourceContent = "NotificationContent";
        internal const string NotificationsExpansion = "NotificationsExpansion";
        internal const string NotificationsLink = "NotificationsLink";
        internal const string NotificationType = "NotificationType";
        internal const string Number = "Number";
        internal const string Occupation = "Occupation";
        internal const string Occurrences = "Occurrences";
        internal const string Offer = "Offer";
        internal const string OfferDetail = "OfferDetail";
        internal const string OfferDetails = "OfferDetails";
        internal const string OfferDetailsId = "OfferDetailsId";
        internal const string OfferId = "OfferId";
        internal const string OfferName = "OfferName";
        internal const string Offers = "Offers";
        internal const string OffersLinks = "OffersLinks";
        internal const string OffsetFrom = "OffsetFrom";
        internal const string OffsetTo = "OffsetTo";
        internal const string OwnerResourceLink = "OwnerResourceLink";
        internal const string PackageId = "PackageId";
        internal const string PackageLink = "PackageLink";
        internal const string PackageName = "PackageName";
        internal const string PackageType = "PackageType";
        internal const string ParentEntryExpansion = "ParentEntryExpansion";
        internal const string ParentEntryLink = "ParentEntryLink";
        internal const string ParentId = "ParentId";
        internal const string Participant = "Participant";
        internal const string Participants = "Participants";
        internal const string Payload = "Payload";
        internal const string PayloadEntry = "PayloadEntry";
        internal const string PeopleExpansion = "ContactsExpansion";
        internal const string PeopleLink = "ContactsLink";
        internal const string PeopleResource = "Contacts";
        internal const string PermaLink = "PermaLink";
        internal const string Person = "Person";
        internal const string PersonalEmail = "PersonalEmail";
        internal const string PersonalIM = "PersonalIM";
        internal const string Pets = "Pets";
        internal const string phoneNumbers = "phoneNumbers";
        internal const string PhoneNumbers = "PhoneNumbers";
        internal const string PhoneType = "PhoneType";
        internal const string PhotosExpansion = "PhotosExpansion";
        internal const string PhotosLink = "PhotosLink";
        internal const string PhotosOfExpansion = "PhotosOfExpansion";
        internal const string PhotosOfLink = "PhotosOfLink";
        internal const string PicturePointer = "PicturePointer";
        internal const string PlacesLived = "PlacesLived";
        internal const string PlayVideoActivity = "PlayVideoActivity";
        internal const string PluralField = "PluralField";
        internal const string PortableContact = "PortableContact";
        internal const string PortableContactCollection = "PortableContactCollection";
        internal const string postalCode = "postalCode";
        internal const string PostalCode = "PostalCode";
        internal const string preferredUserName = "preferredUserName";
        internal const string PreferredUserName = "PreferredUserName";
        internal const string PresenceIconLink = "PresenceIconLink";
        internal const string PresenceStatusText = "PresenceStatusText";
        internal const string PreviewLink = "PreviewLink";
        internal const string PreviousLink = "PreviousLink";
        internal const string PrivacyPolicyLink = "PrivacyPolicyLink";
        internal const string Profession = "Profession";
        internal const string Profile = "Profile";
        internal const string ProfileAddress = "ProfileAddress";
        internal const string ProfileBase = "ProfileBase";
        internal const string ProfileResource = "Profile";
        internal const string ProfileResourceContent = "ProfileContent";
        internal const string Profiles = "Profiles";
        internal const string ProfilesExpansion = "ProfilesExpansion";
        internal const string ProfilesLink = "ProfilesLink";
        internal const string ProfileType = "ProfileType";
        internal const string Properties = "properties";
        internal const string Published = "Published";
        internal const string PublisherName = "PublisherName";
        internal const string QuotaInformation = "QuotaInformation";
        internal const string QuotaUsed = "QuotaUsed";
        internal const string RateProductActivity = "RateProductActivity";
        internal const string Rating = "Rating";
        internal const string RecurrenceDescription = "RecurrenceDescription";
        internal const string region = "region";
        internal const string Relationship = "Relationship";
        internal const string RelationshipStatus = "RelationshipStatus";
        internal const string RelationshipStatusType = "RelationshipStatusType";
        internal const string RelationshipType = "RelationshipType";
        internal const string ReleaseStatus = "ReleaseStatus";
        internal const string ReminderTimeInMin = "ReminderTimeInMin";
        internal const string RepliesCount = "RepliesCount";
        internal const string RepliesExpansion = "RepliesExpansion";
        internal const string RepliesLink = "RepliesLink";
        internal const string RequiresDelegation = "RequiresDelegation";
        internal const string Reserved = "reserved";
        internal const string Resource = "Resource";
        internal const string ResourceCollection = "ResourceCollection";
        internal const string ResourceContent = "ResourceContent";
        internal const string ResourceETag = "ResourceETag";
        internal const string ResourceExpansion = "ResourceExpansion";
        internal const string ResourceFeedLink = "ResourceFeedLink";
        internal const string ResourceFeedsLink = "ResourceFeedsLink";
        internal const string ResourceLink = "ResourceLink";
        internal const string ResourceType = "ResourceType";
        internal const string ReviewedObject = "ReviewedObject";
        internal const string Role = "Role";
        internal const string RoleType = "RoleType";
        internal const string RootDocument = "RootDocument";
        internal const string RootDocumentLink = "RootDocumentLink";
        internal const string RootDocumentUrl = "RootDocumentUrl";
        internal const string Rule = "Rule";
        internal const string RuntimeEndpointLink = "RuntimeEndpointLink";
        internal const string SaveAchievementActivity = "SaveAchievementActivity";
        internal const string SaveHighScoreActivity = "SaveHighScoreActivity";
        internal const string SaveProductActivity = "SaveProductActivity";
        internal const string Scheme = "Scheme";
        internal const string Scope = "Scope";
        internal const string ScriptLink = "ScriptLink";
        internal const string SelfLink = "SelfLink";
        internal const string SenseOfHumor = "SenseOfHumor";
        internal const string Service = "Service";
        internal const string ServiceDescription = "ServiceDescription";
        internal const string ServiceDocumentResource = "ServiceDocument";
        internal const string ServiceProperties = "ServiceProperties";
        internal const string ServiceResource = "ServiceResource";
        internal const string ServiceStatus = "ServiceStatus";
        internal const string ServiceType = "ServiceType";
        internal const string SharingLevel = "SharingLevel";
        internal const string SharingPermission = "SharingPermission";
        internal const string SignedInUser = "SignedInUser";
        internal const string SignificantOther = "SignificantOther";
        internal const string Size = "Size";
        internal const string SlideShowLink = "SlideShowLink";
        internal const string SocialProfile = "SocialProfile";
        internal const string SocialProfileContent = "SocialProfileContent";
        internal const string sorted = "sorted";
        internal const string Src = "Src";
        internal const string startIndex = "startIndex";
        internal const string StartTimeZoneOffset = "StartTimeZoneOffset";
        internal const string State = "State";
        internal const string StaticOffers = "StaticOffers";
        internal const string Status = "Status";
        internal const string StatusComment = "StatusComment";
        internal const string StatusLink = "StatusLink";
        internal const string StatusProfile = "StatusProfile";
        internal const string StatusText = "StatusText";
        internal const string Street = "Street";
        internal const string streetAddress = "streetAddress";
        internal const string StreetAddress = "StreetAddress";
        internal const string SubscribableUrl = "SubscribableUrl";
        internal const string Subscription = "Subscription";
        internal const string SubscriptionLink = "SubscriptionLink";
        internal const string SubscriptionResourceContent = "SubscriptionContent";
        internal const string SuccesfulRequest = "SuccesfulRequest";
        internal const string Suffix = "Suffix";
        internal const string Summary = "Summary";
        internal const string SupportLink = "SupportLink";
        internal const string Sync = "Sync";
        internal const string SyncActivity = "SyncActivity";
        internal const string SyncContentStagingAreaLink = "SyncContentStagingAreaLink";
        internal const string SyncEntriesLink = "SyncEntriesLink";
        internal const string SyncExpansion = "SyncExpansion";
        internal const string SyncLink = "SyncLink";
        internal const string SyncObjectActivityResource = "SyncObjectActivity";
        internal const string SyncObjectActivityResourceContent = "ActivityContent";
        internal const string SyncObjectExpansion = "SyncObjectExpansion";
        internal const string SyncObjectLink = "SyncObjectLink";
        internal const string SyncObjectName = "SyncObjectName";
        internal const string SyncObjectResource = "SyncObject";
        internal const string SyncObjectResourceContent = "SyncObjectContent";
        internal const string SyncObjectsExpansion = "SyncObjectsExpansion";
        internal const string SyncObjectsLink = "SyncObjectsLink";
        internal const string SyncObjectType = "SyncObjectType";
        internal const string SyncState = "SyncState";
        internal const string TagCount = "TagCount";
        internal const string Tagger = "Tagger";
        internal const string TaggerProfilesExpansion = "TaggerProfilesExpansion";
        internal const string TaggerProfilesLink = "TaggerProfilesLink";
        internal const string TaggingEnabled = "TaggingEnabled";
        internal const string TagPersonActivity = "TagPersonActivity";
        internal const string TagResource = "Tag";
        internal const string TagResourceContent = "TagContent";
        internal const string tags = "tags";
        internal const string Tags = "Tags";
        internal const string TagsExpansion = "TagsExpansion";
        internal const string TagsLink = "TagsLink";
        internal const string TargetLink = "TargetLink";
        internal const string TargetTitle = "TargetTitle";
        internal const string Text = "Text";
        internal const string ThrottledRequest = "ThrottledRequest";
        internal const string Thumbnail_104X104C = "Thumbnail_104X104C";
        internal const string Thumbnail_176X176 = "Thumbnail_176X176";
        internal const string Thumbnail_213X213C = "Thumbnail_213X213C";
        internal const string Thumbnail_320X320 = "Thumbnail_320X320";
        internal const string Thumbnail_48X48 = "Thumbnail_48X48";
        internal const string Thumbnail_48X48C = "Thumbnail_48X48C";
        internal const string Thumbnail_600X450 = "Thumbnail_600X450";
        internal const string Thumbnail_800X600 = "Thumbnail_800X600";
        internal const string Thumbnail_96X96 = "Thumbnail_96X96";
        internal const string ThumbnailImageLink = "ThumbnailImageLink";
        internal const string Thumbnails = "Thumbnails";
        internal const string Timeout = "Timeout";
        internal const string TimeZone = "TimeZone";
        internal const string TimeZoneComponent = "TimeZoneComponent";
        internal const string TimeZoneComponents = "TimeZoneComponents";
        internal const string Title = "Title";
        internal const string TotalRequest = "TotalRequest";
        internal const string totalResults = "totalResults";
        internal const string Triggers = "Triggers";
        internal const string type = "type";
        internal const string Type = "Type";
        internal const string UninstallReturnLink = "UninstallReturnLink";
        internal const string University = "University";
        internal const string UniversityEndYear = "UniversityEndYear";
        internal const string UniversityStartYear = "UniversityStartYear";
        internal const string updated = "updated";
        internal const string Updated = "Updated";
        internal const string updatedSince = "updatedSince";
        internal const string Uri = "Uri";
        internal const string Url = "Url";
        internal const string urls = "urls";
        internal const string Urls = "Urls";
        internal const string UserData = "UserData";
        internal const string UserDataBuffer = "UserDataBuffer";
        internal const string UserDataInfoset = "UserDataInfoset";
        internal const string UserInterfaceLink = "UserInterfaceLink";
        internal const string UtcEndTime = "UtcEndTime";
        internal const string utcOffset = "utcOffset";
        internal const string UtcOffset = "UtcOffset";
        internal const string UtcStartTime = "UtcStartTime";
        internal const string UxLink = "UxLink";
        internal const string value = "value";
        internal const string Value = "Value";
        internal const string VendorLink = "VendorLink";
        internal const string Version = "Version";
        internal const string Visibility = "Visibility";
        internal const string WantPets = "WantPets";
        internal const string Watermark = "Watermark";
        internal const string WebreadyImageLink = "WebreadyImageLink";
        internal const string Website = "Website";
        internal const string WhenTaken = "WhenTaken";
        internal const string Width = "Width";
        internal const string WindowsLiveId = "WindowsLiveID";
        internal const string Work = "Work";
        internal const string WorkAddress = "WorkAddress";
        internal const string WorkAddress2 = "WorkAddress2";
        internal const string WorkEmail = "WorkEmail";
        internal const string WorkFax = "WorkFax";
        internal const string WorkPager = "WorkPager";
        internal const string WorkPhone = "WorkPhone";
        internal const string WorkProfile = "WorkProfile";
        internal const string WorkProfileContent = "WorkProfileContent";
        internal const string X0 = "X0";
        internal const string Y0 = "Y0";
    }
}

