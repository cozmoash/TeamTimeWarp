using System;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Rest.Tests.Controllers.Users
{
    [TestFixture]
    public class WhenAUserStartsARestingPhase : UserControllerTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAUserStartsARestingPhase()
        {
            Controller.Post(Request,TimeWarpCommand.Rest);

            FakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0, 5);
            _result = Controller.Get(Request);
        }

        [Test]
        public void ThenTheirTimeWarpStateIsSetToResting()
        {
            Assert.IsNotNull(_result);
            Assert.AreEqual(AccountId, _result.AccountId);
            Assert.AreEqual(TimeWarpState.Resting, _result.State);            
        }

        [Test]
        public void ThenTheirProgressIsCalculated()
        {
            Assert.AreEqual(0.050000000000000044d, _result.Progress);
            Assert.AreEqual(TimeSpan.FromMilliseconds(095), _result.TimeLeft);
        }

        [Test]
        public void ThenTheRestingStartTimeIsCorrect()
        {
            Assert.AreEqual(new DateTime(2000, 12, 12, 12, 12, 0, 0), _result.PeriodStartTime);
        }       
    }
}