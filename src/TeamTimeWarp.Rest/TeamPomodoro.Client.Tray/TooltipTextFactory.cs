using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public class TooltipTextFactory : ITooltipTextFactory
    {
        public string Get(TimeWarpState timeWarpState, int minutesRemaining)
        {
            if (timeWarpState == TimeWarpState.None)
                return "Team Time Warp";

            return string.Concat(timeWarpState.ToString(), ' ', minutesRemaining, " mins remaining");
        }
    }
}