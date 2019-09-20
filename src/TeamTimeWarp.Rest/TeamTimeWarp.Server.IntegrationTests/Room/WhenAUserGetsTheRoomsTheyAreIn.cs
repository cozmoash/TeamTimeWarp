using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenAUserGetsTheRoomsTheyAreIn : IntegrationTestBase
    {
        private readonly IEnumerable<RoomInfo> _rooms;

        public WhenAUserGetsTheRoomsTheyAreIn()
        {
            CreateAccount();
            var room = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            JoinRoom(room.Id);
            _rooms = GetRoomsForLoggedInUser();
        }

        [Test]
        public void ThenTheUserIsInTheRoom()
        {
            Assert.IsNotNull(_rooms.SingleOrDefault());
        }
    }
}