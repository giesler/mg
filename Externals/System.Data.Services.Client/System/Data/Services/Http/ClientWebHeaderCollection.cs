namespace System.Data.Services.Http
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    internal sealed class ClientWebHeaderCollection : WebHeaderCollection
    {
        private System.Net.WebHeaderCollection innerCollection;
        private System.Net.HttpWebRequest request;

        internal ClientWebHeaderCollection(System.Net.WebHeaderCollection collection)
        {
            Debug.Assert(collection != null, "collection can't be null.");
            this.innerCollection = collection;
        }

        internal ClientWebHeaderCollection(System.Net.WebHeaderCollection collection, System.Net.HttpWebRequest request)
        {
            Debug.Assert(collection != null, "collection can't be null.");
            this.innerCollection = collection;
            this.request = request;
        }

        public override ICollection<string> AllKeys
        {
            get
            {
                return this.innerCollection.AllKeys;
            }
        }

        public override int Count
        {
            get
            {
                return this.innerCollection.Count;
            }
        }

        public override string this[HttpRequestHeader header]
        {
            get
            {
                return this[HttpHeaderToName.RequestHeaderNames[header]];
            }
            set
            {
                this[HttpHeaderToName.RequestHeaderNames[header]] = value;
            }
        }

        public override string this[string name]
        {
            get
            {
                return this.innerCollection[name];
            }
            set
            {
                if (name != "Content-Length")
                {
                    if (name == "Accept-Charset")
                    {
                        Debug.Assert(value == "UTF-8", "Asking for AcceptCharset different thatn UTF-8.");
                    }
                    else if (name == "Cookie")
                    {
                        if (this.request != null)
                        {
                            System.Net.CookieContainer container = new System.Net.CookieContainer();
                            container.SetCookies(this.request.RequestUri, value);
                            this.request.CookieContainer=container;
                        }
                        else
                        {
                            this.innerCollection[name]=value;
                        }
                    }
                    else
                    {
                        this.innerCollection[name]=value;
                    }
                }
            }
        }
    }
}

