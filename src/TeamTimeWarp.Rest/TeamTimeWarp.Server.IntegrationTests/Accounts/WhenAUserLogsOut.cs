using System.Net;
using NUnit.Framework;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenAUserLogsOut : IntegrationTestBase
    {
        public WhenAUserLogsOut()
        {
            CreateAccount();
            Logout();
        }
        
        [Test]
        public void ThenFurtherCallsResultInForbidden()
        {
            Assert.AreEqual(HttpStatusCode.Forbidden, StartWork().StatusCode);
        }
    }
}