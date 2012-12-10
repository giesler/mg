namespace System.Data.Services.Http
{
    using System;

    internal class HeaderInfo
    {
        internal readonly string HeaderName;
        internal readonly bool IsRequestRestricted;

        internal HeaderInfo(string name, bool requestRestricted)
        {
            this.HeaderName = name;
            this.IsRequestRestricted = requestRestricted;
        }
    }
}

