using System;

namespace TimeManager.Client.Tray
{
    public class TimeChangedEventArgs : EventArgs
    {
        private readonly int _minutesRemaining;

        public TimeChangedEventArgs(int minutesRemaining)
        {
            _minutesRemaining = minutesRemaining;
        }

        public int MinutesRemaining
        {
            get { return _minutesRemaining; }
        }
    }
}