using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class AuthenticationSessionClassMap : ClassMap<AuthenticationSession>
    {
        public AuthenticationSessionClassMap()
        {
            Table("AuthenticationSession");
            Id(auth => auth.Id).GeneratedBy.HiLo("NH_HiLo", "NextHi", "1000", "TableKey = 'AuthenticationSession'");
            References(auth => auth.Account);
            Map(auth => auth.LastValidation).Not.Nullable();
            Map(auth => auth.Token).Nullable();
        }

    }
}