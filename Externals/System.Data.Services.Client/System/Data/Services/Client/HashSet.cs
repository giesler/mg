namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class HashSet<T> : Dictionary<T, T>, IEnumerable<T>, IEnumerable where T: class
    {
        public HashSet()
        {
        }

        public HashSet(IEqualityComparer<T> comparer) : base(comparer)
        {
        }

        public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : base(comparer)
        {
            this.UnionWith(collection);
        }

        public bool Add(T value)
        {
            if (!base.ContainsKey(value))
            {
                base.Add(value, value);
                return true;
            }
            return false;
        }

        public bool Contains(T value)
        {
            return base.ContainsKey(value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return base.Keys.GetEnumerator();
        }

        public bool Remove(T value)
        {
            return base.Remove(value);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            foreach (T local in other)
            {
                this.Add(local);
            }
        }
    }
}

