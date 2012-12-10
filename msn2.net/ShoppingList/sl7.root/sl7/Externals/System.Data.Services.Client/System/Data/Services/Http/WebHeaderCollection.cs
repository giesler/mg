namespace System.Data.Services.Http
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal abstract class WebHeaderCollection
    {
        protected WebHeaderCollection()
        {
        }

        public virtual void Set(HttpRequestHeader header, string value)
        {
            this[header] = value;
        }

        public abstract ICollection<string> AllKeys { get; }

        public abstract int Count { get; }

        public abstract string this[HttpRequestHeader header] { get; set; }

        public abstract string this[string name] { get; set; }
    }
}

