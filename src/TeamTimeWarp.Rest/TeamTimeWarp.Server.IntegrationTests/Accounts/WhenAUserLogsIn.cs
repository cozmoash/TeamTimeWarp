using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenAUserLogsIn : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAUserLogsIn()
        {
            CreateAccount();
            Logout();
            Login();
            StartWork();
            _result = GetCurrentState();
        }

        [Test]
        public void ThenAUserCanPerformAnAction()
        {
            Assert.AreEqual(TimeWarpState.Working, _result.State);
        }
    }
}