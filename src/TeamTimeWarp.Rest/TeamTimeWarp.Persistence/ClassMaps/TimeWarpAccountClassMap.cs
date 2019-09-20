using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class TimeWarpAccountClassMap : ClassMap<Account>
    {
        public TimeWarpAccountClassMap()
        {
            Table("Account");
            Id(a => a.Id).GeneratedBy.HiLo("NH_HiLo", "NextHi", "1000", "TableKey = 'Account'");
            Map(a => a.Name).Not.Nullable();
            Map(a => a.Email).Not.Nullable();
            Map(a => a.Password).Not.Nullable();
        }
    }
}