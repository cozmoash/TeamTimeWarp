using System;

namespace TeamTimeWarp.Client.Tray
{
    public class TooltipTextChangedEventArgs : EventArgs
    {
        public TooltipTextChangedEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}