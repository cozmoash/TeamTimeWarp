using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenAnAccountIsCreatedWithNoPassword : IntegrationTestBase
    {
        private readonly UserStateInfoResponse _result;

        public WhenAnAccountIsCreatedWithNoPassword()
        {
            CreateAccountWithNoPassword();
            Logout();
            LoginWithNoPassword();
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