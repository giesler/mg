namespace System.Data.Services.Common
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class DataServiceKeyAttribute : Attribute
    {
        private readonly ReadOnlyCollection<string> keyNames;

        public DataServiceKeyAttribute(string keyName)
        {
            Util.CheckArgumentNull<string>(keyName, "keyName");
            Util.CheckArgumentNotEmpty(keyName, "KeyName");
            this.keyNames = new ReadOnlyCollection<string>(new string[] { keyName });
        }

        public DataServiceKeyAttribute(params string[] keyNames)
        {
            Util.CheckArgumentNull<string[]>(keyNames, "keyNames");
            if ((keyNames.Length == 0) || keyNames.Any<string>(delegate(string f)
            {
                return ((f == null) || (f.Length == 0));
            }))
            {
                throw Error.Argument(Strings.DSKAttribute_MustSpecifyAtleastOnePropertyName, "keyNames");
            }
            this.keyNames = new ReadOnlyCollection<string>(keyNames);

        }

        public ReadOnlyCollection<string> KeyNames
        {
            get
            {
                return this.keyNames;
            }
        }
    }
}

