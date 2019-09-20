using System.Drawing;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public interface IIconFactory
    {
        Icon Get(TimeWarpState timeWarpState);
    }
}