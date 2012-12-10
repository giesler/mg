namespace System.Data.Services.Http
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class NameValueFromDictionary : Dictionary<string, List<string>>
    {
        public NameValueFromDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
            Debug.Assert(comparer != null, "comparer != null");
        }

        public void Add(string key, string value)
        {
            List<string> list;
            Debug.Assert(key != null, "key != null");
            Debug.Assert(value != null, "value != null");
            if (base.ContainsKey(key))
            {
                list = base[key];
            }
            else
            {
                list = new List<string>();
            }
            list.Add(value);
            base[key] = list;
        }

        public string Get(string name)
        {
            Debug.Assert(name != null, "name != null");
            string str = null;
            if (base.ContainsKey(name))
            {
                List<string> list = base[name];
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        str = list[i];
                    }
                    else
                    {
                        str = str + list[i];
                    }
                    if (i != (list.Count - 1))
                    {
                        str = str + ",";
                    }
                }
            }
            return str;
        }

        public void Set(string key, string value)
        {
            Debug.Assert(key != null, "key != null");
            Debug.Assert(value != null, "value != null");
            List<string> list = new List<string>();
            list.Add(value);
            base[key] = list;
        }
    }
}

