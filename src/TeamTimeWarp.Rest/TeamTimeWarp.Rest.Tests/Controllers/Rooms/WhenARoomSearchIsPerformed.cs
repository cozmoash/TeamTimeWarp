using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;

namespace TeamTimeWarp.Rest.Tests.Controllers.Rooms
{
    [TestFixture]
    public class WhenARoomSearchIsPerformed
    {
        private readonly IRoomRepository _roomRepository;
        private readonly GlobalRoomInfoController _globalRoomInfoController;
        
        public WhenARoomSearchIsPerformed()
        {
            int numberOfRooms = 20;

            _roomRepository = new MockRoomRepository();
            for (int n = 0; n < numberOfRooms; n++)
            {
                var testRoom1 = new Room(n, "testRoom" + n, new DateTime(2000, 12, 12));
                _roomRepository.Add(testRoom1);    
            }
            
            _globalRoomInfoController = new GlobalRoomInfoController(_roomRepository);
        }

        [Test]
        public void ThenTheMatchedRoomsAreReturned()
        {
            IEnumerable<RoomInfo> searchResult = _globalRoomInfoController.Get("12");

            Assert.AreEqual(1,searchResult.Count());
            Assert.AreEqual("testRoom12",searchResult.Single().Name);
        }

        [Test]
        public void ThenNoRoomsAreReturnedIfTheSearchStringIsEmpty()
        {
            IEnumerable<RoomInfo> searchResult = _globalRoomInfoController.Get(string.Empty);
            Assert.AreEqual(0,searchResult.Count());
        }
    }
}