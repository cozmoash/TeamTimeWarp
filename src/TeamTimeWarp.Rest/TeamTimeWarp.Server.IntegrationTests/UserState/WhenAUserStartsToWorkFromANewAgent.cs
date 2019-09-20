using System.Threading;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.UserState
{
    [TestFixture]
    public class WhenAUserStartsToWorkFromANewAgent : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAUserStartsToWorkFromANewAgent()
        {
            CreateAccount();
            StartRest(TimeWarpAgent.VisualStudio);
            Thread.Sleep(1500);
            StartWork(TimeWarpAgent.LinuxClient);
            _result = GetCurrentState();
        }

        [Test]
        public void ThenTheUserStateIsSetToResting()
        {
            Assert.AreEqual(TimeWarpState.Working, _result.State);
        }

        [Test]
        public void ThenTheTriggeringAgentIsResported()
        {
            Assert.AreEqual(TimeWarpAgent.LinuxClient, _result.TimeWarpAgent);
        }
    }
}