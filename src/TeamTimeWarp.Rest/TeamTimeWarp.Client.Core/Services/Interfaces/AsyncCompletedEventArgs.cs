using System;

namespace TeamTimeWarp.Client.Core.Services
{
    public class AsyncCompletedEventArgs<T> : System.ComponentModel.AsyncCompletedEventArgs
    {
        public AsyncCompletedEventArgs(Exception error, object userState, T result) : base(error, false, userState)
        {
            Result = result;
        }

        public T Result { get; private set; }
    }
}