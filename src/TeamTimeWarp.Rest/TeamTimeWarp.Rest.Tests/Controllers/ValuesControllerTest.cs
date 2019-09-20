using System;
using System.ComponentModel;
using Microsoft.FSharp.Core;
using NUnit.Framework;
using TeamPomodoro.Domain;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Service;
using TimeWarpState = TeamTimeWarp.Domain.Entities.TimeWarpState;


namespace TeamTimeWarp.Rest.Tests.Controllers
{

    internal class FakeNowProvider : INowProvider
    {
        private DateTime _now = new DateTime(2000,12,12,12,12,0);

        public DateTime Now
        {
            get { return _now; }
            set { _now = value; }
        }
    }

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

            ITimeWarpStateCalculator calc = GetTimeWarpStateCalculator();
            var account = new TimeWarpAccount(accountId, "ashley", "beans@beans.beans", "beans");
            var timeWarpUser = new TimeWarpUser(account,calc);

            IUsers users = new Users(new[]{timeWarpUser});

            var controller = new TimeWarpUserController(users,fakeNowProvider, new FakeTimeWarpStatePersistence());

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

            ITimeWarpStateCalculator calc = GetTimeWarpStateCalculator();
            var account = new TimeWarpAccount(accountId, "ashley", "beans@beans.beans", "beans");
            var timeWarpUser = new TimeWarpUser(account, calc);

            IUsers users = new Users(new[] { timeWarpUser });

            var persistence = new FakeTimeWarpStatePersistence();


            var controller = new TimeWarpUserController(users, fakeNowProvider,persistence);

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


        private static TimeWarpStateCalculator GetTimeWarpStateCalculator()
        {
            Func<TimeWarpState, TimeSpan> csFunc = (i) =>
            {
                switch (i)
                {
                    case (TimeWarpState.None):
                        return TimeSpan.Zero;
                    case (TimeWarpState.Resting):
                        return TimeSpan.FromMilliseconds(100);
                    case (TimeWarpState.Working):
                        return TimeSpan.FromMilliseconds(200);
                    default:
                        throw new InvalidEnumArgumentException("i", (int)i, typeof(TimeWarpState));
                }
            };


            var fsharpFunc = FSharpFunc<TimeWarpState, TimeSpan>.FromConverter(
                new Converter<TimeWarpState, TimeSpan>(csFunc));
            var timeWarpStateCalculator = new TimeWarpStateCalculator(fsharpFunc);
            return timeWarpStateCalculator;
        }
    }

    public class FakeTimeWarpStatePersistence : ITimeWarpStatePersistence
    {
        public void SaveState(TimeWarpUserState timeWarpUserState)
        {
            //do nothing.
        }
    }
}
