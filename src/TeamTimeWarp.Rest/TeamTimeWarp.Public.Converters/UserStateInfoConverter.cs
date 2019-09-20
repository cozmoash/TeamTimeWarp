using System;
using System.ComponentModel;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;
using TimeWarpState = TeamTimeWarp.Public.Models.v001.TimeWarpState;

namespace TeamTimeWarp.Public.Converters
{
    public static class UserStateInfoConverter
    {
        public static UserStateInfoResponse ConvertToPublicV001(this TimeWarpUserState timeWarpUser, DateTime queryTime)
        {
            return new UserStateInfoResponse(timeWarpUser.Account.Id, timeWarpUser.Account.Name,queryTime,timeWarpUser.State.ConvertToPublicV001(),
                                             timeWarpUser.PeriodStartTime, timeWarpUser.TimeLeft, timeWarpUser.Progress,timeWarpUser.Account.AccountType == AccountType.Quick, (TimeWarpAgent)timeWarpUser.AgentType);
        }

        public static TimeWarpState ConvertToPublicV001(this Domain.Entities.TimeWarpState state)
        {
            switch (state)
            {
                case(Domain.Entities.TimeWarpState.None):
                    return TimeWarpState.None;
                case(Domain.Entities.TimeWarpState.Resting):
                    return TimeWarpState.Resting;
                case(Domain.Entities.TimeWarpState.Working):
                    return TimeWarpState.Working;
                default:
                    throw new InvalidEnumArgumentException("state", (int)state, typeof(Domain.Entities.TimeWarpState));
            }
        }
    }
}