using System;
using System.Net;
using System.Xml;

internal static class WlcConstants
{
    internal static class ApplicationFlags
    {
        internal const int Certified = 4;
        internal const int Development = 1;
        internal const int Production = 2;
    }

    internal static class AssociationType
    {
        internal const string Account = "Account/";
        internal const string Common = "Common/";
        internal const string Enclosure = "Enclosure/";
        internal const string Storage = "Storage/";
    }

    internal static class AuthConstants
    {
        internal const string AppDelegationToken = "AppDelegationToken";
        internal const string AppToken = "AppToken";
        internal const string AuthorizationHeader = "Authorization";
        internal const string TicketSeparator = ",";
    }

    internal static class Categories
    {
        internal const string Application = "Application";
        internal const string ApplicationClaim = "ApplicationClaim";
        internal const string ApplicationClaims = "ApplicationClaims";
        internal const string ApplicationProperties = "ApplicationProperties";
        internal const string ApplicationProperty = "ApplicationProperty";
        internal const string ApplicationReference = "ApplicationReference";
        internal const string ApplicationReferences = "ApplicationReferences";
        internal const string Applications = "Applications";
        internal const string BatchEntry = "BatchEntry";
        internal const string BatchFeed = "BatchFeed";
        internal const string CachedMapping = "CachedMapping";
        internal const string CachedMappings = "CachedMappings";
        internal const string CachedMembership = "CachedMembership";
        internal const string CachedMemberships = "CachedMemberships";
        internal const string Certificate = "Certificate";
        internal const string Certificates = "Certificates";
        internal const string ConfigEntry = "ConfigEntry";
        internal const string Contact = "Contact";
        internal const string Contacts = "Contacts";
        internal const string CoreObject = "CoreObject";
        internal const string CoreObjects = "CoreObjects";
        internal const string DataEntry = "DataEntry";
        internal const string DataEntryVersion = "DataEntryVersion";
        internal const string DataEntryVersions = "DataEntryVersions";
        internal const string DataFeed = "DataFeed";
        internal const string DataFeedDescriptor = "DataFeedDescriptor";
        internal const string DataFeedError = "DataFeedError";
        internal const string DataFeedErrors = "DataFeedErrors";
        internal const string DataFeeds = "DataFeeds";
        internal const string Datagram = "Datagram";
        internal const string Datagrams = "Datagrams";
        internal const string Device = "Device";
        internal const string DeviceClaim = "DeviceClaim";
        internal const string DeviceClaims = "DeviceClaims";
        internal const string DeviceConnectivityBatch = "DeviceConnectivityBatch";
        internal const string DeviceConnectivityBatches = "DeviceConnectivityBatches";
        internal const string DeviceConnectivityEntries = "DeviceConnectivityEntries";
        internal const string DeviceConnectivityEntry = "DeviceConnectivityEntry";
        internal const string Devices = "Devices";
        internal const string EnclosureState = "EnclosureState";
        internal const string EnclosureStates = "EnclosureStates";
        internal const string Endpoint = "Endpoint";
        internal const string Endpoints = "Endpoints";
        internal const string File = "File";
        internal const string Folder = "Folder";
        internal const string Identities = "Identities";
        internal const string Identity = "Identity";
        internal const string Invitation = "Invitation";
        internal const string Invitations = "Invitations";
        internal const string Mapping = "Mapping";
        internal const string Mappings = "Mappings";
        internal const string Member = "Member";
        internal const string Members = "Members";
        internal const string NewsConfigEntry = "NewsConfigEntry";
        internal const string NewsEntry = "NewsEntry";
        internal const string Notification = "Notification";
        internal const string NotificationQueue = "NotificationQueue";
        internal const string NotificationQueueDescriptor = "NotificationQueueDescriptor";
        internal const string NotificationQueues = "NotificationQueues";
        internal const string PendingMember = "PendingMember";
        internal const string PendingMembers = "PendingMembers";
        internal const string Properties = "Properties";
        internal const string Property = "Property";
        internal const string RecycledItem = "RecycledItem";
        internal const string RecycledItems = "RecycledItems";
        internal const string SessionInvitation = "SessionInvitation";
        internal const string SessionInvitations = "SessionInvitations";
        internal const string SignedInIdentities = "SignedInIdentities";
        internal const string SignedInIdentity = "SignedInIdentity";
        internal const string StorageIdentities = "StorageIdentities";
        internal const string StorageIdentity = "StorageIdentity";
        internal const string StorageServiceDocument = "StorageServiceDocument";
        internal const string Subscription = "Subscription";
        internal const string Subscriptions = "Subscriptions";
        internal const string UserAutoSignIn = "UserAutoSignIn";
        internal const string UserCachedWithPassword = "UserCachedWithPassword";
        internal const string UserLastSignedIn = "UserLastSignedIn";
        internal const string UserProfile = "UserProfile";
    }

    internal static class CoreObjectConstraints
    {
        internal const int CoreObjectDefaultTombstoneRetentionPeriod = -1;
        internal const int CoreObjectMaxCategoryCount = 5;
        internal const int CoreObjectMaxUserLinks = 5;
        internal const int CoreObjectTitleLength = 0x100;
        internal const int CoreObjectTypeLength = 0x20;
    }

    internal static class CreateMemberArguments
    {
        public const string InvitationPendingMember = "p";
        public const string InvitationSecret = "s";
    }

    internal static class DataEntryConstraints
    {
        internal const int DataEntryMaxBinaryContentLength = 0x1000;
        internal const int DataEntryMaxCategoryCount = 5;
        internal const int DataEntryMaxUserLinks = 5;
        internal const int DataEntryTitleLength = 0x100;
    }

    internal static class DataFeedDescriptorConstraints
    {
        internal const int DataFeedDescriptorDefaultTombstoneRetentionPeriod = -1;
    }

    internal static class DeviceConnectivityCategory
    {
        internal const string EndpointEntry = "Endpoint";
        internal const string MoeEntry = "MOE";
        internal const string P2PTestEntry = "P2PTest";
        internal const string RacEntry = "RAC";
    }

    internal static class DeviceConnectivityDeviceType
    {
        internal const string MacType = "Mac";
        internal const string PCType = "PC";
    }

    internal static class EntityIdTranslationNamespaces
    {
        internal const string WlcAuthPartners = "Wlc-Authorization-Partners";
        internal const string WlcPartners = "Wlc-Partners";
        internal const string WlcResourceTypes = "Wlc-ResourceTypes";
    }

    internal static class ETagConstants
    {
        internal const long IfMatchStarCursor = -2L;
    }

    internal static class ExtendedStatusCodes
    {
        internal static class ApiErrors
        {
            internal const string MembershipCreationFailed = "MembershipCreationFailed";
            internal const string MembershipDeletionFailed = "MembershipDeletionFailed";
            internal const string OptimisticConcurrencyFailed = "OptimisticConcurrencyFailed";
        }

        internal static class Authorization
        {
            internal const string AuthTypeUnknownOrNotAllowed = "AuthTypeUnknownOrNotAllowed";
            internal const string PuidAuthIdentityNotFound = "PuidAuthIdentityNotFound";
            internal const string PuidAuthIdentityRetrievalFailed = "PuidAuthIdentityRetrievalFailed";
            internal const string PuidAuthUserNotProvisioned = "PuidAuthUserNotProvisioned";
            internal const string SecretAuthInvalid = "SecretAuthInvalid";
            internal const string SecretAuthNotAllowedOnMethod = "SecretAuthNotAllowedOnMethod";
        }

        internal static class Feed
        {
            internal const string NotMappedToCloud = "FeedIsNotMappedToCloud";
            internal const string TooLarge = "FeedTooLarge";
        }

        internal static class Invitation
        {
            internal const string AlreadyMember = "InvitationAlreadyMember";
            internal const string AlreadyUsed = "InvitationAlreadyUsed";
            internal const string BadSecret = "InvitationUnauthorized";
            internal const string Expired = "InvitationExpired";
            internal const string FailedToExtractIdentityInfo = "FailedToExtractIdentityInfo";
            internal const string IncorrectRole = "IncorrectRole";
            internal const string Invalid = "InvitationInvalid";
            internal const string MaxInvitesLimitReached = "MaxInvitesLimitReached";
            internal const string MaxNumberOfUsersReached = "MaxNumberOfUsersReached";
            internal const string NotInAllowLists = "NotInAllowLists";
            internal const string NotInvitedAs = "InvitationNotInvitedAs";
            internal const string Revoked = "InvitationRevoked";
            internal const string TooManyMembers = "InvitationTooManyMembers";
            internal const string UserProvisioningIsClosed = "UserProvisioningIsClosed";
        }

        internal static class PendingMembers
        {
            internal const string InvitationCreationFailed = "InvitationCreationFailed";
            internal const string LiveIdLookupFailed = "LiveIdLookupFailed";
            internal const string NonLiveIdUserCannotAccept = "NonLiveIdUserCannotAccept";
            internal const string OrphanInvitationCreationFailed = "OrphanInvitationCreationFailed";
            internal const string StorageIdentityLookupFailed = "StorageIdentityLookupFailed";
        }

        internal static class Quota
        {
            internal const string EnclosureQuotaExceeded = "EnclosureQuotaExceeded";
            internal const string ItemsAutoGhosted = "ItemsAutoGhosted";
            internal const string MetadataQuotaExceeded = "MetadataQuotaExceeded";
        }

        internal static class Sse
        {
            internal const string IncomingItemInconsistentWithConflictList = "IncomingItemInconsistentWithConflictList";
            internal const string StateUnmatchedDuringNonSseSyncBypass = "StateUnmatchedDuringNonSseSyncBypass";
        }

        internal static class Throttling
        {
            internal const string LimitExceeded = "RateLimitExceeded";
        }
    }

    internal static class FeedHandlerType
    {
        internal const string FileSystem = "FileSystem";
    }

    internal static class Formats
    {
        internal const string Atom10 = "Atom10";
        internal const string Binary = "Binary";
        internal const string BinaryFormat0 = "v0";
        internal const string DefaultBinaryFormatVersion = "v0";
        internal const string Rss20 = "Rss20";
    }

    internal static class FormatStrings
    {
        internal const string QueryStringExpirationTimeFormat = "yyyyMMddHHmmss";
    }

    internal static class Fragments
    {
        internal const string Feed = "#feed";
    }

    internal static class GetPendingMemberArguments
    {
        public const string DontRedirect = "dr";
        public const string InvitationSecret = "s";
    }

    internal static class HashAlgorithms
    {
        internal const string MD5 = "MD5";
        internal const string SHA256 = "SHA256";
    }

    internal static class HttpHeaders
    {
        internal const string AcceptedInstanceManipulationMethodsHeaderName = "A-IM";
        internal const string AllowNonSyncStateChange = "AllowNonSyncStateChange";
        internal const string Audience = "Audience";
        internal const string AutoGhostEntriesOverQuota = "Auto-Ghost-Entries";
        internal const string CompleteEnclosureCount = "CompleteEnclosureCount";
        internal const string CompleteEnclosureSize = "CompleteEnclosureSize";
        internal const string ContentDispositionAttachment = "attachment";
        internal const string ContentDispositionFilename = "filename";
        internal const string ContentDispositionHeaderName = "Content-Disposition";
        internal const string ContentDispositionInline = "inline";
        internal const string ContentLengthHeaderName = "Content-Length";
        internal const string ContentLocationHeaderName = "Content-Location";
        internal const string ContentRangeHeaderName = "Content-Range";
        internal const string ContentTypeHeaderName = "Content-Type";
        internal const string Cookie = "Cookie";
        internal const string ETagHeaderName = "ETag";
        internal const string FullFeedCursor = "full-feed-etag";
        internal const string FullFeedWithTombstonesEncoding = "with-tombstones";
        internal const string IfMatchHeaderName = "If-Match";
        internal const string IfMatchStarHeader = "*";
        internal const string IfModifiedSinceHeaderName = "If-Modified-Since";
        internal const string IfNoneMatchHeaderName = "If-None-Match";
        internal const string IncompleteEnclosureCount = "IncompleteEnclosureCount";
        internal const string IncompleteEnclosureSize = "IncompleteEnclosureSize";
        internal const string IncrementalFeedEncoding = "feed";
        internal const string IncrementalFeedWithTombstonesEncoding = "feed-with-tombstones";
        internal const string InstanceManipulationMethodsHeaderName = "IM";
        internal const string LastModifiedHeaderName = "Last-Modified";
        internal const string LocationHeaderName = "Location";
        internal const string MethodOverride = "HTTP-Method-Override";
        internal const string PagedReadHeaderName = "PagedRead";
        internal const string ParkRequest = "ParkRequest";
        internal const string Puid = "Puid";
        internal const string RangeHeaderName = "Range";
        internal const string RelatedFeedCursor = "related-feed-etag";
        internal const string SlugHeaderName = "Slug";
        internal const string StatelessClientHeaderName = "StatelessClient";
        internal const string WlcAuthorizationPartnerId = "Wlc-Authorization-PartnerId";
        internal const string WlcContentLengthHeaderName = "Wlc-Content-Length";
        internal const string WlcContentTypeHeaderName = "Wlc-Content-Type";
        internal const string WlcDeviceCertificate = "Wlc-Device-Certificate";
        internal const string WlcDevicePuid = "Wlc-Device-Puid";
        internal const string WlcExtendedStatus = "Wlc-Extended-Status";
        internal const string WlcFilenameHeaderName = "Wlc-Filename";
        internal const string WlcHashAlgorithmHeaderName = "Wlc-Hash-Algorithm";
        internal const string WlcHashHeaderName = "Wlc-Hash";
        internal const string WlcIdentityAppPuid = "Wlc-Identity-AppPuid";
        internal const string WlcIdentityCid = "Wlc-Identity-Cid";
        internal const string WlcIdentityCountry = "Wlc-Identity-Country";
        internal const string WlcIdentityFirstName = "Wlc-Identity-FirstName";
        internal const string WlcIdentityLastName = "Wlc-Identity-LastName";
        internal const string WlcIdentityMemberName = "Wlc-Identity-MemberName";
        internal const string WlcIdentityPuid = "Wlc-Identity-Puid";
        internal const string WlcIdentityTrustedSite = "Wlc-Identity-TrustedSite";
        internal const string WlcJobService = "Wlc-JobService";
        internal const string WlcOffsetHeaderName = "Wlc-Offset";
        internal const string WlcPartnerId = "Wlc-Partner-Id";
        internal const string WlcPredecessorHashHeaderName = "Wlc-Predecessor-Hash";
        internal const string WlcPreserveTicket = "PreserveTicket";
        internal const string WlcQuotaGhostingLevelHeaderName = "Wlc-Quota-GhostingLevel";
        internal const string WlcQuotaReservationHeaderName = "Wlc-Quota-Reservation";
        internal const string WlcRelayTicket = "Wlc-Relay-Ticket";
        internal const string WlcSafeAgentHeaderName = "Wlc-Safe-Agent";
        internal const string WlcTotalSize = "Wlc-Total-Size";
        internal const string WlcUserUILanguage = "Wlc-UserUILanguage";
    }

    internal static class HttpHeadersValues
    {
        internal const string LanguageEnglish = "en";
        internal const string LanguageFrench = "fr";
        internal const string LanguageJapanese = "ja";
        internal const string OnlyIfCached = "only-if-cached";
    }

    internal static class HttpMethods
    {
        internal const string Delete = "DELETE";
        internal const string Get = "GET";
        internal const string Head = "HEAD";
        internal const string Options = "OPTIONS";
        internal const string Post = "POST";
        internal const string Put = "PUT";
    }

    internal static class IdentityFlags
    {
        internal const uint AllowAlternateBasedCoreObjects = 1;
    }

    internal static class InvitationErrors
    {
        public const HttpStatusCode InvitationAccepted = HttpStatusCode.Gone;
        public const HttpStatusCode InvitationRevoked = HttpStatusCode.Forbidden;
    }

    internal static class MappingConstraints
    {
        internal const int MappingLocalPathLength = 0x1000;
        internal const int MappingSpecialFolderNameLength = 0x100;
        internal const int MappingUriLength = 0x200;
    }

    internal static class MappingType
    {
        internal const string Cloud = "Cloud";
        internal const string Device = "Device";
    }

    internal static class MediaTypes
    {
        internal const string ApplicationXml = "application/xml";
        internal const string Atom = "application/atom+xml";
        internal const string AtomEntry = "application/atom+xml;type=entry";
        internal const string AtomFeed = "application/atom+xml;type=feed";
        internal const string Binary = "application/octet-stream";
        internal const string BinaryXml = "binary/xml";
        internal const string EnclosureBlock = "application/wlc-block";
        internal const string EnclosureBlockList = "application/wlc-block-list";
        internal const string EnclosureDescriptor = "application/wlc-enclosure-descriptor";
        internal const string JSon = "application/json";
        internal const string Rss = "application/rss+xml";
        internal const string ServiceDocument = "application/atomsvc+xml";
        internal const string Sse = "application/sse+xml";
        internal const string Xml = "text/xml";
    }

    internal static class NamespacePrefixes
    {
        internal const string Wlc = "wlc";
    }

    internal static class Namespaces
    {
        internal const string App = "http://purl.org/atom/app#";
        internal const string Atom10 = "http://www.w3.org/2005/Atom";
        internal const string Sse = "http://feedsync.org/2007/feedsync";
    }

    internal static class NotificationType
    {
        internal const string Datagram = "Datagram";
        internal const string QueueLost = "QueueLost";
        internal const string QueueOverflow = "QueueOverflow";
        internal const string ResourceChanged = "ResourceChanged";
        internal const string ResourceFetched = "ResourceFetched";
        internal const string SessionInvite = "SessionInvite";
        internal const string Signin = "Signin";
        internal const string Signout = "Signout";
        internal const string StateLost = "StateLost";
    }

    internal static class PendingMemberRedirectQueryParams
    {
        public const string CoreObjectId = "co";
        public const string Error = "error";
        public const string PendingMemberLink = "pm";
    }

    internal static class PendingMemberSecretValues
    {
        public const string InvitationAccepted = "a";
        public const string InvitationRevoked = "r";
    }

    internal static class PerItemActions
    {
        internal const string Ghost = "ghost";
        internal const string Unghost = "unghost";
    }

    internal static class Properties
    {
        internal const string ActivityTime = "ActivityTime";
        internal const string ActivityType = "ActivityType";
        internal const string AllowAnyoneToAccept = "AllowAnyoneToAccept";
        internal const string ApplicationClaimTicket = "ApplicationClaimTicket";
        internal const string ApplicationId = "ApplicationId";
        internal const string ApplicationManifest = "ApplicationManifest";
        internal const string ApplicationWithDelegationTicket = "ApplicationWithDelegationTicket";
        internal const string Attributes = "Attributes";
        internal const string Author = "Author";
        internal const string Authority = "Authority";
        internal const string AutoDownloadEnclosures = "AutoDownloadEnclosures";
        internal const string BytesDownloaded = "BytesDownloaded";
        internal const string BytesToDownload = "BytesToDownload";
        internal const string BytesToUpload = "BytesToUpload";
        internal const string BytesUploaded = "BytesUploaded";
        internal const string CacheCredentials = "CacheCredentials";
        internal const string CanCreate = "CanCreate";
        internal const string CanDelete = "CanDelete";
        internal const string CanRead = "CanRead";
        internal const string CanUpdate = "CanUpdate";
        internal const string Certificate = "Certificate";
        internal const string CID = "CID";
        internal const string Claims = "Claims";
        internal const string ClaimsScope = "ClaimsScope";
        internal const string ClaimsUri = "ClaimsUri";
        internal const string CoalasceHint = "CoalasceHint";
        internal const string CoalesceCount = "CoalesceCount";
        internal const string ConflictPath = "ConflictPath";
        internal const string ConflictResolver = "ConflictResolver";
        internal const string ContentText = "ContentText";
        internal const string ContentType = "ContentType";
        internal const string Contributor = "Contributor";
        internal const string CoreObjectDescription = "CoreObjectDescription";
        internal const string CoreObjectId = "CoreObjectId";
        internal const string CoreObjectName = "CoreObjectName";
        internal const string CoreObjectType = "CoreObjectType";
        internal const string CreateGhostFiles = "CreateGhostFiles";
        internal const string CreationTime = "CreationTime";
        internal const string CreatorId = "CreatorId";
        internal const string CurrentMetadataSize = "CurrentMetadataSize";
        internal const string CurrentSize = "CurrentSize";
        internal const string DataFeedErrorsLink = "DataFeedErrorsLink";
        internal const string DataFeedErrorType = "DataFeedErrorType";
        internal const string DataFeedId = "DataFeedId";
        internal const string DataFeedLink = "DataFeedLink";
        internal const string DataFeedMapping = "DataFeedMapping";
        internal const string DataFeedMappings = "DataFeedMappings";
        internal const string DeviceClaim = "DeviceClaim";
        internal const string DeviceConnectivityEntryId = "DeviceConnectivityEntryId";
        internal const string DeviceConnectivityTicket = "DeviceConnectivityTicket";
        internal const string DeviceId = "DeviceId";
        internal const string DeviceName = "DeviceName";
        internal const string DeviceTicket = "DeviceTicket";
        internal const string DeviceType = "DeviceType";
        internal const string DeviceWithDelegationTicket = "DeviceWithDelegationTicket";
        internal const string Digest = "Digest";
        internal const string DownloadInProgress = "DownloadInProgress";
        internal const string DownloadPriority = "DownloadPriority";
        internal const string Email = "Email";
        internal const string EnclosureDescriptor = "EnclosureDescriptor";
        internal const string EnclosureDescriptors = "EnclosureDescriptors";
        internal const string EnclosureHash = "EnclosureHash";
        internal const string EnclosureId = "EnclosureId";
        internal const string EnclosuresPresent = "EnclosuresPresent";
        internal const string EndpointId = "EndpointId";
        internal const string EntityTag = "EntityTag";
        internal const string EntityTagMapping = "EntityTagMapping";
        internal const string EntityTagMappings = "EntityTagMappings";
        internal const string Entries = "Entries";
        internal const string Entry = "Entry";
        internal const string EntryId = "EntryId";
        internal const string EventType = "EventType";
        internal const string ExclusionFilter = "ExclusionFilter";
        internal const string ExecuteSerially = "ExecuteSerially";
        internal const string Expires = "Expires";
        internal const string ExtensionProperties = "Properties";
        internal const string Extensions = "Extensions";
        internal const string FileSystemParentId = "FileSystemParentId";
        internal const string Flags = "Flags";
        internal const string FriendlyName = "FriendlyName";
        internal const string Generation = "Generation";
        internal const string GhostChildren = "GhostChildren";
        internal const string GlobalLink = "GlobalLink";
        internal const string Hash = "Hash";
        internal const string HashAlgorithm = "HashAlgorithm";
        internal const string Header = "Header";
        internal const string HighWatermark = "HighWatermark";
        internal const string HRef = "href";
        internal const string Id = "Id";
        internal const string IdentityId = "IdentityId";
        internal const string IdentityTicket = "IdentityTicket";
        internal const string InclusionFilter = "InclusionFilter";
        internal const string Inline = "inline";
        internal const string InvitationAcceptLink = "InvitationAcceptLink";
        internal const string InvitationDeclineLink = "InvitationDeclineLink";
        internal const string InvitedAs = "InvitedAs";
        internal const string InvitedAsRole = "InvitedAsRole";
        internal const string InvitedBy = "InvitedBy";
        internal const string IsAdministrativeOffline = "IsAdministrativeOffline";
        internal const string IsAutoGhosted = "IsAutoGhosted";
        internal const string IsDeleted = "IsDeleted";
        internal const string IsMappedToCloud = "IsMappedToCloud";
        internal const string IsOnline = "IsOnline";
        internal const string IsOwner = "IsOwner";
        internal const string IsPresent = "IsPresent";
        internal const string IsShared = "IsShared";
        internal const string ItemETag = "ItemETag";
        internal const string LastUpdateTime = "LastUpdateTime";
        internal const string Length = "Length";
        internal const string LinkTitle = "LinkTitle";
        internal const string LocalDeviceId = "LocalDeviceId";
        internal const string LocalLink = "LocalLink";
        internal const string LocalPath = "LocalPath";
        internal const string LocalSettings = "LocalSettings";
        internal const string MappingLocation = "MappingLocation";
        internal const string MappingType = "MappingType";
        internal const string MapToCloud = "MapToCloud";
        internal const string MaxAge = "MaxAge";
        internal const string MaxEnclosureSize = "MaxEnclosureSize";
        internal const string MaxMetadataSize = "MaxMetadataSize";
        internal const string MaxSize = "MaxSize";
        internal const string MoeVersion = "MoeVersion";
        internal const string Name = "Name";
        internal const string NewsIcon = "NewsIcon";
        internal const string NewsScope = "NewsScope";
        internal const string NewsTarget = "NewsTarget";
        internal const string NewsTargets = "NewsTargets";
        internal const string Operation = "Operation";
        internal const string OwnerIdentityId = "OwnerIdentityId";
        internal const string ParentId = "ParentId";
        internal const string Payload = "Payload";
        internal const string PendingMemberId = "PendingMemberId";
        internal const string PrincipalId = "PrincipalId";
        internal const string ProvisionedOnMyBehalf = "ProvisionedOnMyBehalf";
        internal const string Puid = "Puid";
        internal const string Quota = "Quota";
        internal const string Quotas = "Quotas";
        internal const string Reason = "Reason";
        internal const string RecycledItemId = "RecycledItemId";
        internal const string Reltype = "rel";
        internal const string RemoteGatewayAddress = "RemoteGatewayAddress";
        internal const string RequestHeaders = "RequestHeaders";
        internal const string ResourcePerson = "ResourcePerson";
        internal const string ResponseHeaders = "ResponseHeaders";
        internal const string Role = "Role";
        internal const string Roles = "Roles";
        internal const string Secret = "Secret";
        internal const string ServiceDescription = "ServiceDescription";
        internal const string ServiceProperties = "ServiceProperties";
        internal const string ServiceStatus = "ServiceStatus";
        internal const string ServiceType = "ServiceType";
        internal const string Settings = "Settings";
        internal const string ShouldRecycle = "ShouldRecycle";
        internal const string ShouldRestore = "ShouldRestore";
        internal const string Size = "Size";
        internal const string SpecialFolderName = "SpecialFolderName";
        internal const string StatusCode = "StatusCode";
        internal const string Subject = "Subject";
        internal const string Summary = "Summary";
        internal const string TargetLink = "TargetLink";
        internal const string Ticket = "Ticket";
        internal const string Title = "Title";
        internal const string TombstoneRetentionPeriod = "TombstoneRetentionPeriod";
        internal const string Type = "Type";
        internal const string UploadRequired = "UploadRequired";
        internal const string UserContent = "UserContent";
        internal const string UserId = "UserId";
        internal const string UserLink = "UserLink";
        internal const string UserLinks = "UserLinks";
        internal const string UserName = "UserName";
        internal const string Value = "Value";
        internal const string WatchFiles = "WatchFiles";
        internal const string Watermark = "Watermark";
    }

    internal static class PropertyPrefixes
    {
        internal const string Id = "urn:uuid:";
        internal const string WlcAppId = "wlc:app:id:";
    }

    internal static class QueryString
    {
        public const string AllowServerItemUpdates = "allowServerItemUpdates";
        public const string ApplicationClaimUri = "applicationClaimUri";
        public const string AppUri = "appUri";
        public const string Attachment = "attachment";
        public const string Callback = "$callback";
        public const string ClientFeedState = "state";
        public const string ConflictingEntriesOnly = "conflictingEntriesOnly";
        public const string Email = "email";
        public const string EncodeFilename = "encodeFileName";
        public const string Expand = "$expand";
        public const string ExpirationTime = "expires";
        public const string Filename = "file";
        public const string Filter = "$filter";
        public const string Format = "$format";
        public const string InitialSync = "initialSync";
        public const string Jsonp = "jsonp";
        public const string LastUpdatedTime = "lastupdatedtime";
        public const string MappingOnly = "mappingonly";
        public const string OnlyExtensions = "onlyExtensions";
        public const string OrderBy = "$orderBy";
        public const string OrderByDescending = "desc";
        public const string OriginalPartnerId = "originalPartnerId";
        public const string PaginationCount = "count";
        public const string PaginationLowerBound = "lowerBound";
        public const string PaginationStartKey = "startKey";
        public const string PaginationUpperBound = "upperBound";
        public const string ParentId = "parentId";
        public const string Principal = "principal";
        public const string ProfileImageThumbnail = "Thumbnail";
        public const string ProfileImageType = "profileimagetype";
        public const string ProfileImageWebready = "WebReady";
        public const string PublishDate = "publishdate";
        public const string Puid = "puid";
        public const string QueryKey = "queryKey";
        public const string QueryValue = "queryValue";
        public const string QuotaAuthorityId = "authorityId";
        public const string QuotaEnclosureSize = "enclosureSize";
        public const string QuotaGhostedEnclosureSize1 = "ghostedEnclosureSize1";
        public const string QuotaGhostedEnclosureSize2 = "ghostedEnclosureSize2";
        public const string QuotaMetadataSize = "metadataSize";
        public const string QuotaOperation = "operation";
        public const string ReplicaId = "replicaId";
        public const string ResolveConflicts = "resolveConflicts";
        public const string Scope = "scope";
        public const string Signature = "sig";
        public const string Size = "size";
        public const string Skip = "$skip";
        public const string SortAscending = "asc";
        public const string SortBy = "$orderBy";
        public const string SortByKey = "sortBy";
        public const string SortDescending = "desc";
        public const string SortOrderKey = "sortOrder";
        public const string Title = "title";
        public const string Top = "$top";
        public const string Type = "type";
    }

    internal static class QuotaOperations
    {
        internal const string ChargeQuota = "ChargeQuota";
        internal const string CommitReservation = "CommitReservation";
        internal const string CreateReservation = "CreateReservation";
        internal const string FreeQuota = "FreeQuota";
        internal const string ReleaseReservation = "ReleaseReservation";
    }

    internal static class Reltypes
    {
        internal const string Application = "Account/Application";
        internal const string ApplicationClaim = "Account/ApplicationClaim";
        internal const string ApplicationClaims = "Account/ApplicationClaims";
        internal const string ApplicationProperties = "Account/ApplicationProperties";
        internal const string ApplicationReferences = "Storage/ApplicationReferences";
        internal const string Applications = "Account/Applications";
        internal const string ApplicationSource = "Account/ApplicationSource";
        internal const string ApplicationVendor = "Account/ApplicationVendor";
        internal const string Batch = "batch";
        internal const string Bookmark = "bookmark";
        internal const string CachedMappings = "Account/CachedMappings";
        internal const string CachedMemberships = "Account/CachedMemberships";
        internal const string CompleteEnclosures = "Storage/CompleteEnclosures";
        internal const string Conflicts = "Storage/Conflicts";
        internal const string Contact = "Account/Contact";
        internal const string CoreObject = "Storage/CoreObject";
        internal const string CoreObjects = "Storage/CoreObjects";
        internal const string DataEntry = "Storage/DataEntry";
        internal const string DataFeedErrors = "Storage/DataFeedErrors";
        internal const string DataFeeds = "Storage/DataFeeds";
        internal const string Datagram = "Account/Datagram";
        internal const string Datagrams = "Account/Datagram";
        internal const string Device = "Account/Device";
        internal const string DeviceClaim = "Account/DeviceClaim";
        internal const string DeviceClaims = "Account/DeviceClaims";
        internal const string DeviceConnectivityEntries = "Account/DeviceConnectivityEntries";
        internal const string DeviceConnectivityEntry = "Account/DeviceConnectivityEntry";
        internal const string Edit = "edit";
        internal const string EditMedia = "edit-media";
        internal const string Enclosure = "enclosure";
        internal const string Enclosures = "Enclosure/Enclosures";
        internal const string EnclosureStates = "Storage/EnclosureStates";
        internal const string Endpoint = "Account/Endpoint";
        internal const string Endpoints = "Account/Endpoints";
        internal const string EndpointService = "Account/EndpointService";
        internal const string First = "first";
        internal const string GroupedSubscriptions = "Common/GroupedSubscriptions";
        internal const string Icon = "Storage/Icon";
        internal const string Identity = "Account/Identity";
        internal const string IncompleteEnclosures = "Storage/IncompleteEnclosures";
        internal const string Invitation = "Storage/Invitation";
        internal const string InvitationAccept = "Storage/InvitationAccept";
        internal const string InvitationDecline = "Storage/InvitationDecline";
        internal const string InvitationDisplay = "Storage/InvitationDisplay";
        internal const string Invitations = "Storage/Invitations";
        internal const string InvitedBy = "Account/InvitedBy";
        internal const string Last = "last";
        internal const string Local = "local";
        internal const string Mapping = "Storage/Mapping";
        internal const string Mappings = "Storage/Mappings";
        internal const string Member = "Storage/Member";
        internal const string Members = "Storage/Members";
        internal const string Next = "next";
        internal const string NotificationQueue = "Account/NotificationQueue";
        internal const string NotificationQueues = "Account/NotificationQueues";
        internal const string PendingMember = "Storage/PendingMember";
        internal const string PendingMembers = "Storage/PendingMembers";
        internal const string Previous = "previous";
        internal const string ProfileThumbnail = "Account/ThumbNail";
        internal const string ProfileWebReady = "Account/WebReady";
        internal const string Properties = "Account/Properties";
        internal const string RecycledItem = "Storage/RecycledItem";
        internal const string RecycledItems = "Storage/RecycledItems";
        internal const string Resource = "Common/Resource";
        internal const string Self = "self";
        internal const string SessionInvitation = "Account/SessionInvitation";
        internal const string SessionInvitations = "Account/SessionInvitations";
        internal const string SourceDeviceConnectivity = "Account/SourceDeviceConnectivity";
        internal const string SourcePackage = "Account/Package";
        internal const string Sse = "sse";
        internal const string StorageIdentity = "Storage/Identity";
        internal const string StorageNotificationQueue = "Storage/NotificationQueue";
        internal const string StorageNotificationQueues = "Storage/NotificationQueues";
        internal const string StorageResource = "Storage/Resource";
        internal const string StorageServiceDocument = "Storage/StorageServiceDocument";
        internal const string Subject = "Common/Subject";
        internal const string Subscription = "Common/Subscription";
        internal const string SubscriptionBroker = "Common/SubscriptionBroker";
        internal const string Version = "version";
        internal const string Versions = "versions";
    }

    internal static class ResourceChangeReason
    {
        internal const string Accept = "Accept";
        internal const string Available = "Available";
        internal const string Create = "Create";
        internal const string Delete = "Delete";
        internal const string Fail = "Fail";
        internal const string Offline = "Offline";
        internal const string Online = "Online";
        internal const string Recycle = "Recycle";
        internal const string Reject = "Reject";
        internal const string Resolve = "Resolve";
        internal const string Unknown = "Unknown";
        internal const string Update = "Update";
    }

    internal static class ResourceCollections
    {
        internal const string Applications = "Applications";
        internal const string CachedUsers = "CachedUsers";
        internal const string Certificates = "Certificates";
        internal const string CoreObjects = "CoreObjects";
        internal const string DeviceClaimCleanup = "DeviceClaimCleanup";
        internal const string Devices = "Devices";
        internal const string Identities = "Identities";
        internal const string Memberships = "Memberships";
        internal const string SignedInIdentities = "SignedInIdentities";
        internal const string SubscribeAll = "SubscribeAll";
    }

    internal static class Schemas
    {
        internal const string Wlc = "http://schemas.live.com/2007/01/";
    }

    internal static class Schemes
    {
        internal const string CoreObject = "http://schemas.live.com/2007/01/CoreObjects";
        internal const string DataEntry = "http://schemas.live.com/2007/01/DataEntry";
        internal const string DataFeedDescriptor = "http://schemas.live.com/2007/01/DataFeedDescriptor";
        internal const string Device = "http://schemas.live.com/2007/01/Device";
        internal const string DeviceConnectivityEntry = "http://schemas.live.com/2007/01/DeviceConnectivityEntry";
        internal const string Endpoint = "http://schemas.live.com/2007/01/Endpoint";
        internal const string FeedHandlerType = "http://schemas.live.com/2007/01/FeedHandlerType";
        internal const string NotificationType = "http://schemas.live.com/2007/01/NotificationType";
        internal const string Wlc = "http://schemas.live.com/2007/01/";
    }

    internal static class ServiceProperties
    {
        internal const string AllowAlternateBasedCoreObjects = "rd";
        internal const string AllowTesProperty = "tes";
        internal const string DefaultForNewIdentity = "rd,";
        internal const char Delimiter = ',';
        internal const string MacProperty = "mac1";
        internal const string MobileProperty = "wm1";
    }

    internal static class StringFormats
    {
        internal const string Rfc3339UTCDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
    }

    internal static class Terms
    {
        internal const string CloudData = "CloudData";
        internal const string CoreObject = "CoreObjectType";
        internal const string DataEntryType = "DataEntryType";
        internal const string DataFeedDescriptor = "DataFeedDescriptorType";
        internal const string DeviceConnectivityEntry = "DeviceConnectivityEntry";
        internal const string DeviceType = "DeviceType";
        internal const string EndpointType = "EndpointType";
        internal const string FeedHandlerType = "FeedHandlerType";
        internal const string NotificationType = "NotificationType";
        internal const string ResourceType = "ResourceType";
        internal const string SplCoreObjectType = "SplCoreObjectType";
    }

    internal static class WlcAuthorizationPartnerIds
    {
        public static readonly Guid AdminPartnerId = new Guid("BF5770C7-59A6-4437-B331-43380BF7B6CB");
        public static readonly Guid InternalSyncServicePartnerId = new Guid("AB8CA961-7B09-46B1-8516-080C7CD2A552");
        public static readonly Guid TestSyncServicePartnerId = new Guid("F9523BB5-A3AE-46EC-9676-2311023FA429");
    }

    internal static class WlcPartnerIds
    {
        internal static readonly Guid DefaultExternalId = new Guid("{A775690A-F565-489E-A86B-7CCB9D200EE8}");
        internal const string DefaultExternalName = "LiveMesh";
        internal const byte DefaultInternalId = 0;
        internal static readonly Guid GomezDefaultPrincipalId = new Guid("{6b2367bb-ba7b-4251-bf8c-32d27299a66f}");
        internal static readonly Guid GomezNoThrottlePrincipalId = new Guid("{826b54a5-4754-4921-a5e1-239d8f4c055e}");
        internal static readonly Guid GomezOfficialPrincipalId = new Guid("{787b907f-a0e6-4463-abc6-e97390880cba}");
    }

    internal static class WLCSyndicationVersions
    {
        internal const string Atom10 = "Atom10";
        internal const string Binary = "Binary";
        internal const string Default = "Atom10";
        internal const string Rss20 = "Rss20";
    }

    internal static class WLDS
    {
        internal const string Age = "age";
        internal const string Blog = "blog";
        internal const string Business = "business";
        internal const string City = "city";
        internal const string College1 = "college1";
        internal const string College2 = "college2";
        internal const string Country = "country";
        internal const string Degree = "degree";
        internal const string Email = "email";
        internal const string Emails = "emails";
        internal const string FashionTaste = "fashiontaste";
        internal const string FreeText = "freetext";
        internal const string Gender = "gender";
        internal const string GlobalNickname = "globalnickname";
        internal const string GraduationYear = "graduationyear";
        internal const string HighSchool = "highschool";
        internal const string Home = "home";
        internal const string HomeTown = "hometown";
        internal const string Identity = "identity";
        internal const string InterestedIn = "interestedin";
        internal const string Interests = "interests";
        internal const string LandLine = "landline";
        internal const string Location = "location";
        internal const string Major = "major";
        internal const string Mobile = "mobile";
        internal const string MusicTaste = "musictaste";
        internal const string Name = "name";
        internal const string Occupation = "occupation";
        internal const string Pets = "pets";
        internal const string PlacesLived = "placeslived";
        internal const string PostalCode = "postalcode";
        internal const string SenseOfHumor = "senseofhumor";
        internal const string State = "state";
        internal const string Street = "street";
        internal const string WantPets = "wantpets";
        internal const string WebSite = "website";
        internal const string Work = "work";
    }

    internal static class Workspaces
    {
        internal const string Account = "Account";
        internal const string StorageService = "StorageService";
    }

    internal static class XmlConstants
    {
        internal static XmlQualifiedName XmlBase = new XmlQualifiedName("base", "xml");
        internal const string XmlNS = "xmlns";
    }
}

