namespace Microsoft.Live
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal class WebRequestData
    {
        public Action<Exception> Callback { get; set; }
        public LiveDataContext DataContext { get; set; }
        public bool IsRootDocument { get; set; }
        public WebRequest WebRequest { get; set; }
        //[CompilerGenerated]
        //private Action<Exception> <Callback>k__BackingField;
        //[CompilerGenerated]
        //private LiveDataContext <DataContext>k__BackingField;
        //[CompilerGenerated]
        //private bool <IsRootDocument>k__BackingField;
        //[CompilerGenerated]
        //private System.Net.WebRequest <WebRequest>k__BackingField;

        internal WebRequestData(System.Net.WebRequest webRequest, LiveDataContext dataContext, bool isRootDocument, Action<Exception> callback)
        {
            this.WebRequest = webRequest;
            this.DataContext = dataContext;
            this.IsRootDocument = isRootDocument;
            this.Callback = callback;
        }

        //public Action<Exception> Callback
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Callback>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Callback>k__BackingField = value;
        //    }
        //}

        //public LiveDataContext DataContext
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<DataContext>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<DataContext>k__BackingField = value;
        //    }
        //}

        //public bool IsRootDocument
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<IsRootDocument>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<IsRootDocument>k__BackingField = value;
        //    }
        //}

        //public System.Net.WebRequest WebRequest
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<WebRequest>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<WebRequest>k__BackingField = value;
        //    }
        //}
    }
}

