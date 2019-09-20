using System;

namespace TeamTimeWarp.Client.Core
{
    public class EventArgs<T> : EventArgs
    {
        private readonly T _item;

        public EventArgs(T item)
        {
            _item = item;
        }

        public T Item
        {
            get { return _item; }
        }
    }
}