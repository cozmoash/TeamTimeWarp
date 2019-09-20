using System;
using System.Linq;
using NUnit.Framework;
using TeamPomodoro.Domain;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    [TestFixture]
    public class RoomsControllerTest
    {
        private RoomInfoController _roomInfoController;
        private AccountsCache _accountsCache;
        private RoomsCache _roomsCache;
        private FakeEntityPersistence<Room> _entityPersistence;
        private FakeNowProvider _nowProvider;

        [SetUp]
        public void SetupController()
        {
            //arrange
            var testRoom1 = new Room(1, "testRoom1", new DateTime(2000, 12, 12));
            var testRoom2 = new Room(2, "testRoom2", new DateTime(2001, 12, 12));
            var testAccount = new Account(1, "ashley", "ashley@teamTimeWarp.com", "beanland");

            _roomsCache = new RoomsCache(new[] { testRoom1,testRoom2 });
            _accountsCache = new AccountsCache(new[] { testAccount });

            _entityPersistence = new FakeEntityPersistence<Room>();
            _nowProvider = new FakeNowProvider();

            var roomsController = new RoomInfoController(_roomsCache, _accountsCache, _entityPersistence,
                                                         _nowProvider);
            _roomInfoController = roomsController;
        }
        
        [Test]
        public void GetSpecficRoom()
        {
            //act
            var result = _roomInfoController.Get(1);

            //assert
            Assert.AreEqual(1,result.Id);
            Assert.AreEqual("testRoom1",result.Name);
            Assert.AreEqual(0,result.NumberOfUsers);
        }

        [Test]
        public void CreateNewRoom()
        {
            RoomInfo newRoom = _roomInfoController.Post("new test room");
            Room newRoomEntity;
            _roomsCache.TryGet(0, out newRoomEntity);

            Assert.AreEqual("new test room",newRoom.Name);
            Assert.AreEqual(0,newRoomEntity.NumberOfUsers);
        }

        [Test]
        public void JoinAndThenLeaveRoom()
        {
            AddUserToExistingRoom();
            var roomsController = new RoomInfoController(_roomsCache, _accountsCache, _entityPersistence,
                                                         _nowProvider);
            roomsController.Put(1,1,UserRoomCommand.Leave);
            Room room;
            Assert.IsTrue(_roomsCache.TryGet(1, out room));
            Assert.AreEqual(0, _entityPersistence.SavedItems.Last().NumberOfUsers);
        }

        [Test]
        public void AddUserToExistingRoom()
        {
            _roomInfoController.Put(1,1,UserRoomCommand.Join);

            Room room;
            Assert.IsTrue(_roomsCache.TryGet(1, out room));
            Assert.AreEqual(1,_entityPersistence.SavedItems.Single().NumberOfUsers);
            Assert.AreEqual(1,room.Users.Single().Id);
        }

        [Test]
        public void GetAllRooms()
        {
            var result = _roomInfoController.Get();

            Assert.AreEqual(2,result.Count());
        }

        
    }
}