using System;
using System.ComponentModel;
using RestSharp;

namespace TeamTimeWarp.Client.Core.Services
{
    public static class AsyncCompletedEventArgsExtensions
    {
        public static void Raise(EventHandler<AsyncCompletedEventArgs> eventHandler,
                                          IRestResponse response, object state)
        {
            var exception = response.ErrorException;
            
            //do we need to capture the httpstatuscode?

            EventHandler<AsyncCompletedEventArgs> handler = eventHandler;

            if (handler != null)
                handler(null, new AsyncCompletedEventArgs(exception, false,state));
        }

        public static void Raise<TResult>(EventHandler<AsyncCompletedEventArgs<TResult>> eventHandler,
                                          IRestResponse response) where TResult : class 
        {

            var exception = response.ErrorException;

            TResult deserializedResult = null;
            if(exception == null)
                deserializedResult = JsonHelper.SafeDeserializeObject<TResult>(response.Content);
            
            EventHandler<AsyncCompletedEventArgs<TResult>> handler = eventHandler;
            
            if (handler != null)
                handler(null, new AsyncCompletedEventArgs<TResult>(exception,null,deserializedResult));
        }
    }
}