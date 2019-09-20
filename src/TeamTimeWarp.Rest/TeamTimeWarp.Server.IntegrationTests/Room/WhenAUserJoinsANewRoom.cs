using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    [Ignore]
    public class CreateSfsRoom : IntegrationTestBase
    {
        public CreateSfsRoom()
        {
            CreateAccount();
            CreateRoom("SFS");
        }
    }


    [TestFixture]
    public class WhenAUserJoinsANewRoom : IntegrationTestBase
    {
        private readonly RoomInfo _room1;
        private readonly RoomInfo _room2;

        public WhenAUserJoinsANewRoom()
        {
            CreateAccount();
            StartWork();
            _room1 = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + 1);
            _room2 = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + 2);
            JoinRoom(_room1.Id);
            JoinRoom(_room2.Id);
            
        }

        [Test]
        public void ThenTheUserIsInTheRoom()
        {
            Assert.AreEqual(1, RoomStatus(_room2.Id).Count());
        }

        [Test]
        public void ThenTheUserLeavesTheirOldRoomRoom()
        {
            Assert.AreEqual(0, RoomStatus(_room1.Id).Count());
        }
    }
}