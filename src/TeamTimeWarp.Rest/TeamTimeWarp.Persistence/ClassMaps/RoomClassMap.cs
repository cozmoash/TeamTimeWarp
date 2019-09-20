using FluentNHibernate.Mapping;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.ClassMaps
{
    public class RoomClassMap : ClassMap<Room>
    {
        public RoomClassMap()
        {
            Table("Room");
            Id(r => r.Id).GeneratedBy.HiLo("NH_HiLo", "NextHi", "1000", "TableKey = 'Room'");
            
            Map(r => r.CreationTime).Not.Nullable();
            Map(r => r.Name).Not.Nullable();
            //Map(r => r.Users).Not.Nullable();

            HasManyToMany(x => x.Users)
                .AsSet()
                .Table("RoomAccounts")
                .ParentKeyColumn("Room_Id")
                .ChildKeyColumn("Account_Id")
                .Cascade.All();
        }
    }
}