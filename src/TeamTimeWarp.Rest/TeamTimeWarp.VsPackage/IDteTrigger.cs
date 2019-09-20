using System;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    public interface IDteTrigger : IDisposable
    {
        event EventHandler<EventArgs> OnDteTrigger;
    }
}