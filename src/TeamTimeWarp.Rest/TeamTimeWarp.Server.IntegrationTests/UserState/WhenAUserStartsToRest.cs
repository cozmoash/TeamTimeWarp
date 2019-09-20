using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.UserState
{
    [TestFixture]
    public class WhenAUserStartsToRest : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAUserStartsToRest()
        {
            CreateAccount();
            StartRest(TimeWarpAgent.VisualStudio);
            _result = GetCurrentState();
        }

        [Test]
        public void ThenTheUserStateIsSetToResting()
        {
            Assert.AreEqual(TimeWarpState.Resting, _result.State);
        }

        [Test]
        public void ThenTheTriggeringAgentIsResported()
        {
            Assert.AreEqual(TimeWarpAgent.VisualStudio, _result.TimeWarpAgent);
        }
    }
}