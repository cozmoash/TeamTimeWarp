using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenARoomSearchIsPerformed : IntegrationTestBase
    {
        private readonly IEnumerable<RoomInfo> _result;

        public WhenARoomSearchIsPerformed()
        {
            var roomName = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            CreateAccount();
            CreateRoom(roomName);
            _result = SearchRoom(roomName.Substring(0,1));
        }

        [Test]
        public void ThenAllRoomsWhichMatchTheSearchStringAreReturned()
        {
            Assert.That(_result.Any());
        }
    }
}