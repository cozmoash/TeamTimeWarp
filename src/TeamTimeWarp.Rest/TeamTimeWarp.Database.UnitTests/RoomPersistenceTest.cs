
using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.Persistence.Rooms;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class RoomPersistenceTest
    {
        [Test]
        public void CanAddNewRooms()
        {
            var room1 = new Room(0, TestHelper.RoomName, new DateTime(2000, 12, 12));

            var roomRepository = new RoomRepository();
            roomRepository.Add(room1);

            Assert.That(room1.Id != 0);
        }

        [Test]
        public void CanSearchForRooms()
        {
            var roomName = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var room1 = new Room(0, roomName, new DateTime(2000, 12, 12));

            var roomRepository = new RoomRepository();
            roomRepository.Add(room1);

            var searchResults = roomRepository.GetRooms(roomName);
            Assert.AreEqual(1,searchResults.Count());
        }  

        [Test]
        public void CanAddUserToRoom()
        {
            var room = new Room(0, TestHelper.RoomName, new DateTime(2000, 12, 12));
            var account1 = TestHelper.AccountMock();
            var account2 = TestHelper.AccountMock();
            var accountRepository = new AccountRepository();
            var roomRepository = new RoomRepository();

            roomRepository.Add(room);

            accountRepository.Add(account1);
            accountRepository.Add(account2);

            room.Add(account1);
            room.Add(account2);

            roomRepository.Add(room);

            var roomRepository2 = new RoomRepository();
            Room result = roomRepository2.GetRoom(room.Id);
            
            Assert.AreEqual(2, result.Users.Count());


            var rooms= roomRepository2.GetRooms(account1.Id);

            Assert.AreEqual(1,rooms.Count());

        }
    }
}