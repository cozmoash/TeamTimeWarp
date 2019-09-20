using System;

namespace TeamTimeWarp.Rest.Controllers
{
    public class SessionIdProvider : ISessionIdProvider
    {
        public string GetSessionId()
        {
            var sessionId = Guid.NewGuid().ToString("N");
            sessionId = sessionId.Substring(4, 24);
            return sessionId;
        }
    }
}