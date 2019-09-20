using System;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Rest.Tests.Controllers.Accounts;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;
using TeamTimeWarp.UnitTests;
using TimeWarpState = TeamTimeWarp.Domain.Entities.TimeWarpState;

namespace TeamTimeWarp.Rest.Tests.Controllers.Rooms
{

    [TestFixture]
    public class RoomStateControllerTest
    {
        private RoomStateController _roomStateController;
        private FakeNowProvider _fakeNowProvider;
        private MockRoomRepository _roomRepository;
        private Room _testRoom;
        private UserStateManager _userStateManager;

    
        [TestCase(AccountType.Quick)]
        [TestCase(AccountType.Fake)]
        [TestCase(AccountType.Full)]
        public void CanGetRoomState(AccountType accountType)
        {
            _fakeNowProvider = new FakeNowProvider { Now = new DateTime(2000, 12, 12, 12, 12, 0) };
            var usersCache = new MockUserStateRepository();
            var calc = FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();

            _userStateManager = new UserStateManager(calc, usersCache);

            _testRoom = new Room(2, "testRoom2", new DateTime(2001, 12, 12));

            _roomRepository = new MockRoomRepository();

            _roomRepository.Add(_testRoom);

            _roomStateController = new RoomStateController(_userStateManager, _roomRepository, new RoomRemovalPolicy(), _fakeNowProvider);
            
            var newAccount = new Account(0, TestHelper.NameMock, TestHelper.EmailAddressMock +1, accountType);
            _testRoom.Add(newAccount);
            usersCache.Add(new TimeWarpUserState(0,newAccount, TimeWarpState.Resting, _fakeNowProvider.Now,TimeSpan.FromSeconds(1),0,1));

            var oldAccount = new Account(1, TestHelper.NameMock, TestHelper.EmailAddressMock + 2, accountType);
            _testRoom.Add(oldAccount);
            usersCache.Add(new TimeWarpUserState(1,oldAccount, TimeWarpState.Resting, _fakeNowProvider.Now.Subtract(TimeSpan.FromDays(100)), TimeSpan.FromSeconds(1), 0, 1));

            var state = _roomStateController.Get(2);

            Assert.AreEqual(1,state.Count());
            Assert.AreEqual(0, state.Single().AccountId);
        }


    }

    [TestFixture]
    public class RoomsControllerTest
    {
        private RoomInfoController _roomInfoController;
        private IRoomRepository _roomRepository;
        private IAccountRepository _accountRepository;
        private FakeNowProvider _nowProvider;
        private HttpRequestMessage _request;
        private MockTimeWarpAuthenticationManager _authenticationManager;
        private GlobalRoomInfoController _globalRoomInfoController;

        [SetUp]
        public void SetupController()
        {
            //arrange
            var testRoom1 = new Room(1, "testRoom1", new DateTime(2000, 12, 12));
            var testRoom2 = new Room(2, "testRoom2", new DateTime(2001, 12, 12));
            var testAccount = new Account(1, "ashley", "ashley@teamTimeWarp.com", AccountType.Full);

            _authenticationManager = new MockTimeWarpAuthenticationManager();
            var token = _authenticationManager.AddUser(new AccountPassword(testAccount,""));

            _request = HttpRequestMock.MockRequest();
            _request.Headers.Add("login-token", token);

            _roomRepository = new MockRoomRepository();
            _roomRepository.Add(testRoom1);
            _roomRepository.Add(testRoom2);

            _accountRepository = new MockAccountsRepository();
            _accountRepository.Add(testAccount);

            _nowProvider = new FakeNowProvider();

            var roomsController = new RoomInfoController(_roomRepository, _accountRepository,_authenticationManager,
                                                         _nowProvider);

            _globalRoomInfoController = new GlobalRoomInfoController(_roomRepository);
            _roomInfoController = roomsController;
        }

        [Test]
        public void GetAllRooms()
        {
            var result = _globalRoomInfoController.Get();

            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public void GetSpecficRoom()
        {
            //act
            var result = _globalRoomInfoController.Get(1);

            //assert
            Assert.AreEqual(1,result.Id);
            Assert.AreEqual("testRoom1",result.Name);
            Assert.AreEqual(0,result.NumberOfUsers);
        }

        [Test]
        public void CreateNewRoom()
        {
            RoomInfo newRoom = _roomInfoController.Post(_request,"new test room");
            Room newRoomEntity = _roomRepository.GetRoom(0);

            Assert.AreEqual("new test room",newRoom.Name);
            Assert.AreEqual(0,newRoomEntity.NumberOfUsers);
        }

        [Test]
        public void JoinAndThenLeaveRoom()
        {
            AddUserToExistingRoom();
            var roomsController = new RoomInfoController(_roomRepository,_accountRepository,_authenticationManager,
                                                         _nowProvider);
            roomsController.Post(_request,1,UserRoomCommand.Leave);
            
            Assert.IsTrue(_roomRepository.GetRoom(1) != null);
            Assert.AreEqual(0, _roomRepository.GetRoom(1).NumberOfUsers);
        }

        [Test]
        public void AddUserToExistingRoom()
        {
            _roomInfoController.Post(_request, 1, UserRoomCommand.Join);

            Assert.AreEqual(1, _roomRepository.GetRoom(1).NumberOfUsers);
            Assert.AreEqual(1, _roomRepository.GetRoom(1).Users.Single().Id);
        }


        
    }
}