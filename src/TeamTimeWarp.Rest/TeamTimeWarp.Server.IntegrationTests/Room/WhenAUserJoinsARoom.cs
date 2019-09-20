using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenAUserJoinsARoom : IntegrationTestBase
    {
        private readonly AccountCreationResponse _account;
        private readonly RoomInfo _room;

        public WhenAUserJoinsARoom()
        {
            _account = CreateAccount();
            _room = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            JoinRoom(_room.Id);
            
        }

        [Test]
        public void ThenTheUserIsNotInTheRoomUntilHeMakesAnAction()
        {
            var roomStatus = RoomStatus(_room.Id);
            Assert.AreEqual(0, roomStatus.Count());

            StartWork();
            roomStatus = RoomStatus(_room.Id);

            Assert.AreEqual(_account.AccountId, roomStatus.Single().AccountId);
        }
    }

    [TestFixture]
    public class WhenAQuickLoginAccountJoinsARoomAndLogouts : IntegrationTestBase
    {
        private readonly IEnumerable<UserStateInfoResponse> _roomStatus;
        private readonly AccountCreationResponse _account;

        public WhenAQuickLoginAccountJoinsARoomAndLogouts()
        {
            _account = CreateQuickLoginAccount();
            var room = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            JoinRoom(room.Id);
            Logout();
            _roomStatus = RoomStatus(room.Id);
        }

        [Test]
        public void ThenTheUserIsInTheRoom()
        {
            Assert.AreEqual(0, _roomStatus.Count());
        }
    }

}
