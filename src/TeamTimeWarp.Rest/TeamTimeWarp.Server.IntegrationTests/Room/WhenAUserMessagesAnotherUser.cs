using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    [TestFixture]//need more edge case tests.
    public class WhenAUserMessagesAnotherUser : MessagingIntegrationTestBase
    {
        private ICollection<UserMessageReceipt> _messages;
        private string _message = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

        public WhenAUserMessagesAnotherUser()
        {
            var room = Messaging1.CreateRoom(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));

            Messaging1.JoinRoom(room.Id);
            Messaging2.JoinRoom(room.Id);
            
            Messaging1.SendMessage(Account2.AccountId, _message);
            Thread.Sleep(1000);
            _messages = Messaging2.GetMessages();
        }

        [Test]
        public void ThenTheUserShouldBeAbleToGetThatMessage()
        {
            Assert.AreEqual(_message, _messages.Single().Message);
            Assert.That(_messages.Single().SendTime, Is.Not.EqualTo(default(DateTime)));
            Assert.IsNotNullOrEmpty(_messages.Single().FromName);
        }
    }
}