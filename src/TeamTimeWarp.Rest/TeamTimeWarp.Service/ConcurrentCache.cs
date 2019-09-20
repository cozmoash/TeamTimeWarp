using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TeamTimeWarp.Service
{
    public abstract class ConcurrentCache<TKey,TValue> : IEnumerable<TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _cache;

        protected ConcurrentCache(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if(items == null)
                throw new ArgumentNullException("items");

            _cache = new ConcurrentDictionary<TKey, TValue>(items);
        }

        public bool TryGet(TKey id, out TValue item)
        {
            return _cache.TryGetValue(id, out item);
        }

        public bool TryAdd(TKey id, TValue value)
        {
            return _cache.TryAdd(id, value);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _cache.ToArray().Select(x=>x.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}