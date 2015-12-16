using System;
using System.Collections.Generic;

namespace AxisEngine
{
    public class DoubleKeyMap<K, V>
    {
        private Dictionary<DoubleKey, V> map = new Dictionary<DoubleKey, V>();

        public DoubleKeyMap()
        {
        }

        public IEnumerable<Tuple<K, K>> Keys
        {
            get
            {
                List<Tuple<K, K>> result = new List<Tuple<K, K>>();
                foreach (DoubleKey k in map.Keys)
                {
                    result.Add(k as Tuple<K, K>);
                }
                return result as IEnumerable<Tuple<K, K>>;
            }
        }

        public void Set(K key1, K key2, V val)
        {
            DoubleKey key = new DoubleKey(key1, key2);
            map[key] = val;
        }

        public void Set(Tuple<K, K> key, V val)
        {
            Set(key.Item1, key.Item2, val);
        }

        public bool ContainsKey(K key1, K key2)
        {
            DoubleKey key = new DoubleKey(key1, key2);
            return map.ContainsKey(key);
        }

        public V Get(K key1, K key2)
        {
            DoubleKey key = new DoubleKey(key1, key2);
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            throw new KeyNotFoundException();
        }

        public V Get(Tuple<K, K> key)
        {
            return Get(key.Item1, key.Item2);
        }

        public IEnumerable<K> GetPairedKeys(K key)
        {
            List<K> result = new List<K>();
            foreach (DoubleKey k in map.Keys)
            {
                if (k.Contains(key))
                    result.Add(k.GetOther(key));
            }
            return result as IEnumerable<K>;
        }

        public void RemoveKey(K key)
        {
            List<DoubleKey> foundKeys = new List<DoubleKey>();
            foreach (DoubleKey k in map.Keys)
                if (k.Contains(key))
                    foundKeys.Add(k);
            foreach (DoubleKey k in foundKeys)
                map.Remove(k);
        }

        // class to hold a pair of values where order doesn't matter.
        // (item1, item2) == (item2, item1) -> true
        private class DoubleKey : Tuple<K, K>
        {
            public DoubleKey(K item1, K item2) : base(item1, item2)
            {
            }

            public bool Contains(K key)
            {
                return Item1.Equals(key) || Item2.Equals(key);
            }

            public override bool Equals(object obj)
            {
                if (obj is DoubleKey)
                {
                    DoubleKey other = obj as DoubleKey;
                    if (Item1.Equals(other.Item1) && Item2.Equals(other.Item2) ||
                        Item1.Equals(other.Item2) && Item2.Equals(other.Item1))
                        return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Item1.GetHashCode() * Item2.GetHashCode();
            }

            public K GetOther(K key)
            {
                if (Item1.Equals(key))
                    return Item2;
                if (Item2.Equals(key))
                    return Item1;
                throw new KeyNotFoundException();
            }
        }
    }
}