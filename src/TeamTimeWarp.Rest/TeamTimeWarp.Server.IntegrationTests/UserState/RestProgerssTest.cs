using System;
using System.Threading;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.UserState
{
    [TestFixture]
    public class WhenAUserIsResting : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result1;
        private readonly UserStateInfoResponse _result2;
        private readonly UserStateInfoResponse _result3;

        public WhenAUserIsResting()
        {
            CreateAccount();
            StartRest();
            _result1 = GetCurrentState();
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            _result2 = GetCurrentState();
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            _result3 = GetCurrentState();
        }

        [Test]
        public void ThenTheirStatusIsSetToWorking()
        {
            Assert.AreEqual(TimeWarpState.Resting, _result1.State);
            Assert.AreEqual(TimeWarpState.Resting, _result2.State);
            Assert.AreEqual(TimeWarpState.Resting, _result3.State);
        }

        [Test]
        public void ThenTheirProgressIncreases()
        {
            Assert.That(_result2.Progress < _result3.Progress);
            Assert.That(_result2.Progress < _result3.Progress);
        }
    }
}