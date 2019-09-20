using System;
using System.Collections.Generic;

namespace TeamTimeWarp.Domain.Entities
{
    public class FuncEqualityCompare<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equal;
        private readonly Func<T,int> _hashCode;

        public FuncEqualityCompare(Func<T, T, bool> equal, Func<T,int> hashCode)
        {
            _equal = equal;
            _hashCode = hashCode;
        }


        public bool Equals(T x, T y)
        {
            return _equal(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashCode(obj);
        }
    }
}