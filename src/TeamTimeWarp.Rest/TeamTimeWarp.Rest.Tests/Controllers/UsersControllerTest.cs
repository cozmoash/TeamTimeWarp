using System;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        [Test]
        public void Get()
        {
            // Arrange
            long accountId = 1;

            var fakeNowProvider = new FakeNowProvider();
            fakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0);

            ITimeWarpStateCalculator calc = FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();
            var account = new Account(accountId, "ashley", "beans@beans.beans", "beans");
            var timeWarpUser = new TimeWarpUser(account,calc);

            var usersCache = new UsersCache(new[]{timeWarpUser});

            var controller = new UserStateController(usersCache,fakeNowProvider, new FakeTimeWarpStatePersistence());

            //act
            fakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0, 5);
            var result = controller.Get(accountId);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountId, result.AccountId);
            Assert.AreEqual(Public.Models.v001.TimeWarpState.None, result.State);
            
        }

        [Test]
        public void Post()
        {
            // Arrange
            long accountId = 1;

            var fakeNowProvider = new FakeNowProvider();
            fakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0);

            ITimeWarpStateCalculator calc = FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();
            var account = new Account(accountId, "ashley", "beans@beans.beans", "beans");
            var timeWarpUser = new TimeWarpUser(account, calc);

            var usersCache = new UsersCache(new[] { timeWarpUser });

            var persistence = new FakeTimeWarpStatePersistence();


            var controller = new UserStateController(usersCache,fakeNowProvider,persistence);

            //act
            
            controller.Post(accountId,TimeWarpCommand.Rest);

            fakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0, 5);
            var result = controller.Get(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountId, result.AccountId);
            Assert.AreEqual(Public.Models.v001.TimeWarpState.Resting, result.State);
            Assert.AreEqual(TimeSpan.FromMilliseconds(095), result.TimeLeft);

            Assert.AreEqual(0.050000000000000044d, result.Progress);
            Assert.AreEqual(new DateTime(2000, 12, 12, 12, 12, 0, 0), result.PeriodStartTime);
        }


       
    }
}