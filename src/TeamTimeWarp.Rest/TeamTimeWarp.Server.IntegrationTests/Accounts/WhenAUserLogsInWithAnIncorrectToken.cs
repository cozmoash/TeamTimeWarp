using System.Net;
using NUnit.Framework;
using RestSharp;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenAUserLogsInWithAnIncorrectToken : IntegrationTestBase
    {
        public WhenAUserLogsInWithAnIncorrectToken()
        {
            LoginToken = "wrong-token";
        }

        [Test]
        public void ThenAUserCanPerformAnAction()
        {
            IRestResponse result = StartWork();

            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }
    }
}