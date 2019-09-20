using System;
using System.Globalization;
using System.Net;
using NUnit.Framework;
using RestSharp;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenAUserMessagesANonExistantUser : MessagingIntegrationTestBase
    {
        private string _message = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        private readonly IRestResponse _result;

        public WhenAUserMessagesANonExistantUser()
        {
            _result = Messaging1.SendMessage(99999999, _message);
        }

        [Test]
        public void ThenAnErrorShouldBeReturned()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,_result.StatusCode);
        }
    }
}