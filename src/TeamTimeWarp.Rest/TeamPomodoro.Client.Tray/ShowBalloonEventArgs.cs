using System;
using System.Drawing;

namespace TeamTimeWarp.Client.Tray
{
    public class ShowBalloonEventArgs : EventArgs
    {
        public ShowBalloonEventArgs(int timeout, string title, string message, Icon trayIcon, string toastImageResource)
        {
            ToastImageResource = toastImageResource;
            TrayIcon = trayIcon;
            Message = message;
            Title = title;
            Timeout = timeout;
        }

        public Icon TrayIcon { get; private set; }
        public int Timeout { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string ToastImageResource { get; private set; }
    }
}