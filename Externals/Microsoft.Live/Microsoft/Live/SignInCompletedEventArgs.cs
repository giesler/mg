namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class SignInCompletedEventArgs : AsyncCompletedEventArgs
    {
        public AppAuthentication AuthInfo { get; set; }
        public bool Succeeded { get; set; }

        //[CompilerGenerated]
        //private AppAuthentication <AuthInfo>k__BackingField;
        //[CompilerGenerated]
        //private bool <Succeeded>k__BackingField;

        internal SignInCompletedEventArgs(bool success, AppAuthentication authInfo, object state) : base(authInfo.Error, false, state)
        {
            this.Succeeded = success;
            this.AuthInfo = authInfo;
        }

        //public AppAuthentication AuthInfo
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<AuthInfo>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<AuthInfo>k__BackingField = value;
        //    }
        //}

        //public bool Succeeded
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Succeeded>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Succeeded>k__BackingField = value;
        //    }
        //}
    }
}

