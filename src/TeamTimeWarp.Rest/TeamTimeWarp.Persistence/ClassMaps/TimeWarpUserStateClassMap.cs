using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class TimeWarpUserStateClassMap : ClassMap<TimeWarpUserState>
    {
        public TimeWarpUserStateClassMap()
        {
            Table("UserState");
            Id(u => u.Id).GeneratedBy.HiLo("NH_HiLo", "NextHi", "1000", "TableKey = 'Account'");
            //HasOne(u => u.Account);
            //References(u => u.Account, "AccountId");
            References(x => x.Account);
            Map(u => u.PeriodStartTime,"PeriodStartTime");
            Map(u => u.State, "TimeWarpState");
            Map(u => u.TimeLeft, "TimeLeft");
            Map(u => u.AgentType, "AgentType");
        }
    }
}