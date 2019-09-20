using System;
using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public class RoomInfo
    {
        public RoomInfo(int id, string name, DateTime creationDate, int numberOfUsers)
        {
            NumberOfUsers = numberOfUsers;
            CreationTime = creationDate;
            Name = name;
            Id = id;
        }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public DateTime CreationTime { get; private set; }

        [DataMember]
        public int NumberOfUsers { get; private set; }

    }
}
