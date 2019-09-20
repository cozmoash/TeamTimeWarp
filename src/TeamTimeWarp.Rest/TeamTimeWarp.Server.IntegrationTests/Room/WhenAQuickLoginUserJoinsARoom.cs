using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenAQuickLoginUserJoinsARoom : IntegrationTestBase
    {
        private readonly IEnumerable<UserStateInfoResponse> _roomStatus;
        private readonly AccountCreationResponse _account;

        public WhenAQuickLoginUserJoinsARoom()
        {
            _account = CreateQuickLoginAccount();
            StartWork();
            var room = CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            JoinRoom(room.Id);
            _roomStatus = RoomStatus(room.Id);
           
        }

        [Test]
        public void ThenTheUserIsInTheRoom()
        {
            Assert.AreEqual(_account.AccountId, _roomStatus.Single().AccountId);
        }

        [Test]
        public void ThenTheUserIsMarkedAsAQuickLoginUser()
        {
            Assert.IsTrue(_roomStatus.Single().IsQuickLoginUser);
        }
    }
}