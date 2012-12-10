namespace Microsoft.Live
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;

    public class AppAuthentication
    {
        public string AccessToken { get; set; }
        public string ClientVerifier { get; set; }
        public Exception Error { get; set; }
        public string Expiry { get; set; }
        public Collection<Offer> GrantedOffers { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public string UserId { get; set; }
        //[CompilerGenerated]
        //private string <AccessToken>k__BackingField;
        //[CompilerGenerated]
        //private string <ClientVerifier>k__BackingField;
        //[CompilerGenerated]
        //private Exception <Error>k__BackingField;
        //[CompilerGenerated]
        //private string <Expiry>k__BackingField;
        //[CompilerGenerated]
        //private Collection<Offer> <GrantedOffers>k__BackingField;
        //[CompilerGenerated]
        //private string <RefreshToken>k__BackingField;
        //[CompilerGenerated]
        //private string <Scope>k__BackingField;
        //[CompilerGenerated]
        //private string <UserId>k__BackingField;

        public AppAuthentication()
        {
        }

        public AppAuthentication(IDictionary<string, string> authInfo)
        {
            if (authInfo == null)
            {
                throw new ArgumentNullException("authInfo");
            }
            foreach (string str in authInfo.Keys)
            {
                string grantedOffers = authInfo[str];
                string str3 = str;
                if (str3 != null)
                {
                    if (!(str3 == "AccessToken"))
                    {
                        if (str3 == "RefreshToken")
                        {
                            goto Label_0085;
                        }
                        if (str3 == "Expiry")
                        {
                            goto Label_008E;
                        }
                        if (str3 == "UID")
                        {
                            goto Label_0097;
                        }
                        if (str3 == "Scope")
                        {
                            goto Label_00A0;
                        }
                    }
                    else
                    {
                        this.AccessToken = grantedOffers;
                    }
                }
                continue;
            Label_0085:
                this.RefreshToken = grantedOffers;
                continue;
            Label_008E:
                this.Expiry = grantedOffers;
                continue;
            Label_0097:
                this.UserId = grantedOffers;
                continue;
            Label_00A0:
                this.Scope = grantedOffers;
                this.GrantedOffers = WrapProtocolHelper.ParseOffers(grantedOffers);
            }
        }

        public AppAuthentication(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }

        public IDictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("UID", this.UserId);
            dictionary.Add("Expiry", this.Expiry);
            dictionary.Add("AccessToken", this.AccessToken);
            dictionary.Add("RefreshToken", this.RefreshToken);
            dictionary.Add("Scope", this.Scope);
            return dictionary;
        }

        //public string AccessToken
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<AccessToken>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<AccessToken>k__BackingField = value;
        //    }
        //}

        //internal string ClientVerifier
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ClientVerifier>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ClientVerifier>k__BackingField = value;
        //    }
        //}

        //public Exception Error
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Error>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<Error>k__BackingField = value;
        //    }
        //}

        public DateTime ExpirationDate
        {
            get
            {
                long num;
                if (long.TryParse(this.Expiry, out num))
                {
                    return AuthConstants.Wrap.BaseDateTime.AddSeconds((double) num);
                }
                return new DateTime();
            }
        }

        //internal string Expiry
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Expiry>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Expiry>k__BackingField = value;
        //    }
        //}

        //public Collection<Offer> GrantedOffers
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<GrantedOffers>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<GrantedOffers>k__BackingField = value;
        //    }
        //}

        //public string RefreshToken
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<RefreshToken>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<RefreshToken>k__BackingField = value;
        //    }
        //}

        //internal string Scope
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Scope>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Scope>k__BackingField = value;
        //    }
        //}

        //public string UserId
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<UserId>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<UserId>k__BackingField = value;
        //    }
        //}
    }
}

