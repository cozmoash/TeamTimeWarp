using System;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public class UserMessageEventArgs : EventArgs
    {
        
        private readonly TimeWarpStateUserMessage _updatedState;

        public UserMessageEventArgs(TimeWarpStateUserMessage updatedState, TimeWarpAgent timeWarpAgent)
        {
            TimeWarpAgent = timeWarpAgent;
            _updatedState = updatedState;
        }

        public TimeWarpAgent TimeWarpAgent { get; private set; }

        public TimeWarpStateUserMessage UpdatedState
        {
            get { return _updatedState; }
        }
    }
}