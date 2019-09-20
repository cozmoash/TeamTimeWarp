using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.UserState
{
    [TestFixture]
    public class WhenAUserStartsToWork : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAUserStartsToWork()
        {
            CreateAccount();
            StartWork();
            _result = GetCurrentState();
        }

        [Test]
        public void ThenTheUserStateIsSetToWorking()
        {
            Assert.AreEqual(TimeWarpState.Working,_result.State); 
        }
    }
}