using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public interface ITooltipTextFactory
    {
        string Get(TimeWarpState timeWarpState, int minutesRemaining);
    }
}