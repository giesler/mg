namespace Microsoft.Live
{
    using System;

    internal static class LiveConstants
    {
        public const string ServiceDocExpansion = "?$expand=Contacts,Profiles,Activities,Documents,Photos,Calendar,Sync";

        internal static class AtomNames
        {
            public const string Accept = "accept";
            public const string Base = "base";
            public const string Collection = "collection";
            public const string ContentType = "application/xml";
            public const string Href = "href";
            public const string Inline = "inline";
            public const string Service = "service";
            public const string ServiceDocumentContentType = "application/atomsvc+xml";
            public const string Title = "title";
            public const string Workspace = "workspace";
        }

        internal static class HttpHeaders
        {
            public const string Authorization = "Authorization";
        }

        internal static class Namespaces
        {
            public const string App = "http://www.w3.org/2007/app";
            public const string Atom = "http://www.w3.org/2005/Atom";
            public const string DataServices = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            public const string DataServicesMetadata = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            public const string Xml = "http://www.w3.org/XML/1998/namespace";
        }

        internal static class Url
        {
            public const char Seperator = '/';
        }
    }
}

