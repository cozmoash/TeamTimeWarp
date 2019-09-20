using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class UserMessageClassMap : ClassMap<UserMessage>
    {
        public UserMessageClassMap()
        {
            Table("UserMessage");
            Id(a => a.Id).GeneratedBy.Identity();
            References(x => x.FromAccount,"FromAccount_Id").Not.Nullable();
            References(x => x.ToAccount,"ToAccount_Id").Not.Nullable();
            Map(x => x.SendTime).Not.Nullable();
            Map(x => x.TextMessage).Not.Nullable();
            Map(x => x.HasBeenReceived).Not.Nullable();
        }
    }
}