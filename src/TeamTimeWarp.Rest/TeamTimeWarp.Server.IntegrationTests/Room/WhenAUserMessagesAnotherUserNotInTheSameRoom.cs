using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using NUnit.Framework;
using RestSharp;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]
    public class WhenAUserMessagesAnotherUserNotInTheSameRoom : MessagingIntegrationTestBase
    {
        private ICollection<UserMessageReceipt> _messages;
        private string _message = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        private IRestResponse _sendResult;

        public WhenAUserMessagesAnotherUserNotInTheSameRoom()
        {
            _sendResult = Messaging1.SendMessage(Account2.AccountId, _message);
            Thread.Sleep(1000);
            _messages = Messaging2.GetMessages();
        }

        [Test]
        public void ThenTheUserShouldNotGetThatMessage()
        {
            Assert.IsEmpty(_messages);
        }

        [Test]
        public void ThenAnErrorShouldBeReturnedToTheUserWhoSentTheMessage()
        {
            Assert.AreEqual(HttpStatusCode.NotFound, _sendResult.StatusCode);
        }
    }
}