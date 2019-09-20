using System;
using System.ComponentModel;

namespace TeamTimeWarp.Client.Core.Services
{

    public static class AsyncCompletedEventArgsExt
    {
        public static bool IsErrored(this AsyncCompletedEventArgs asyncCompletedEventArgs )
        {
            return asyncCompletedEventArgs.Error != null;
        }
    }

    //public class LoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    //{
    //    public LoginCompletedEventArgs(Exception error) : base(error, false,null)
    //    {
    //    }
    //}
}