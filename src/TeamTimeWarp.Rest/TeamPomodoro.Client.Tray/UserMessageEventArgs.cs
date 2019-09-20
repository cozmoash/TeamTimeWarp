using System;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public class UserMessageEventArgs : EventArgs
    {
        
        private readonly UserMessage _updatedState;

        public UserMessageEventArgs(UserMessage updatedState, TimeWarpAgent timeWarpAgent)
        {
            TimeWarpAgent = timeWarpAgent;
            _updatedState = updatedState;
        }

        public TimeWarpAgent TimeWarpAgent { get; private set; }

        public UserMessage UpdatedState
        {
            get { return _updatedState; }
        }
    }
}