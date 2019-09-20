using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class AccountPasswordClassMap : ClassMap<AccountPassword>
    {
        public AccountPasswordClassMap()
        {
            Table("AccountPassword");
            Id(u => u.Id).GeneratedBy.HiLo("NH_HiLo", "NextHi", "1000", "TableKey = 'Account'");
            References(x => x.Account);
            Map(a => a.Password).Nullable();
        }
    }
}