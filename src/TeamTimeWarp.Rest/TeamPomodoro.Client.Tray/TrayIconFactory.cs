using System;
using System.Drawing;
using System.IO;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public class TrayIconFactory : IIconFactory
    {
        public Icon Get(TimeWarpState timeWarpState)
        {
            return timeWarpState == (TimeWarpState.Working)
                       ? new Icon(Path.Combine(Environment.CurrentDirectory, "Working.ico"), 40, 40)
                       : new Icon(Path.Combine(Environment.CurrentDirectory, "Transmission.ico"), 40, 40);
        }
    }
}