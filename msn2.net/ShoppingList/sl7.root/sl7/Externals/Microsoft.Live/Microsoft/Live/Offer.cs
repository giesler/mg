namespace Microsoft.Live
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class Offer
    {

        public string Action { get; set; }
        public DateTime Expiration { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        //[CompilerGenerated]
        //private string <Action>k__BackingField;
        //[CompilerGenerated]
        //private DateTime <Expiration>k__BackingField;
        //[CompilerGenerated]
        //private string <Name>k__BackingField;
        //[CompilerGenerated]
        //private string <Namespace>k__BackingField;
        private static string[] OfferNames = new string[] { "Messenger", "WL_Activities", "WL_Calendar", "WL_Contacts", "WL_Photos", "WL_Documents", "WL_Profiles", "WL_Test", "WL" };

        public Offer()
        {
        }

        internal Offer(string offerString)
        {
            this.ParseOfferString(offerString);
        }

        private void ParseOfferString(string offerString)
        {
            long num2;
            if (string.IsNullOrEmpty(offerString))
            {
                throw new ArgumentNullException("offerString");
            }
            string[] strArray = offerString.Split(new char[] { AuthConstants.Wrap.OfferSeparator[1], AuthConstants.Wrap.OfferSeparator[2] });
            if (strArray.Length < 2)
            {
                throw new ArgumentException("Invalid offer string.");
            }
            string str = strArray[0];
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Invalid offer string.");
            }
            foreach (string str2 in OfferNames)
            {
                int index = str.IndexOf(str2);
                if (index != -1)
                {
                    this.Namespace = str.Substring(0, index);
                    this.Name = str2;
                    break;
                }
            }
            this.Action = strArray[1];
            if ((strArray.Length == 3) && long.TryParse(strArray[2], out num2))
            {
                this.Expiration = AuthConstants.Wrap.BaseDateTime.AddSeconds((double) num2);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Namespace).Append(this.Name).Append(AuthConstants.Wrap.OfferSeparator[1]).Append(this.Action);
            return builder.ToString();
        }

        //public string Action
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Action>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Action>k__BackingField = value;
        //    }
        //}

        //public DateTime Expiration
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Expiration>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<Expiration>k__BackingField = value;
        //    }
        //}

        //public string Name
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Name>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Name>k__BackingField = value;
        //    }
        //}

        //public string Namespace
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Namespace>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Namespace>k__BackingField = value;
        //    }
        //}
    }
}

