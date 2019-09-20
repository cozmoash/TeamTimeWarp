using System;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence.Accounts;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class UserMessagePersistenceTest
    {
        private string _uniqueTestString;
        private Account _fromaccount;
        private Account _toaccount;

        [SetUp]
        public void Setup()
        {
            _uniqueTestString = DateTime.Now.Ticks.ToString();

            _fromaccount = new Account(0, "FromAccount" + _uniqueTestString, "FromEmail" + _uniqueTestString,
                                          AccountType.Full);
            _toaccount = new Account(0, "ToAccount" + _uniqueTestString, "ToEmail" + _uniqueTestString,
                                        AccountType.Full);

            var accountRepository = new AccountRepository();
            accountRepository.Add(_fromaccount);
            accountRepository.Add(_toaccount);
        }
        
        [Test]
        public void CanSetStatusToHasBeenReceived()
        {
            var sendTime = DateTime.Now;
            var userMessage = new UserMessage(_toaccount, _fromaccount, sendTime, "message" + _uniqueTestString);

            UserMessageRepository userMessageRepository = new UserMessageRepository();

            userMessageRepository.Add(userMessage);
            Assert.That(userMessage.Id > 0);

            userMessage.HasBeenReceived = true;

            userMessageRepository.Add(userMessage);

            Assert.AreEqual(true,userMessage.HasBeenReceived);
        }


        [Test]
        public void CanGetTheLastUserMessage()
        {
            var randomRccount = new Account(0, "RandomAccount" + _uniqueTestString, "ToEmail" + _uniqueTestString,
                                        AccountType.Full);


            var accountRepository = new AccountRepository();
            accountRepository.Add(_fromaccount);
            accountRepository.Add(_toaccount);
            accountRepository.Add(randomRccount);

            var sendTime = DateTime.Now;
            var userMessage1 = new UserMessage(_toaccount, _fromaccount, sendTime, "message" + _uniqueTestString + 1)
                {HasBeenReceived = true};
            var userMessage2 = new UserMessage(_toaccount, _fromaccount, sendTime, "message" + _uniqueTestString + 2) 
                { HasBeenReceived = true };
            var userMessage3 = new UserMessage(randomRccount, _fromaccount, sendTime, "message" + _uniqueTestString + 3);
            var userMessage4 = new UserMessage(_toaccount, _fromaccount, sendTime, "message" + _uniqueTestString + 3);



            UserMessageRepository userMessageRepository = new UserMessageRepository();

            userMessageRepository.Add(userMessage1);
            userMessageRepository.Add(userMessage2);
            userMessageRepository.Add(userMessage3);
            userMessageRepository.Add(userMessage4);

            var result = userMessageRepository.GetAllPendingMessagesForAccount(_toaccount.Id);

            var singleMessage = result.Single();

            Assert.AreEqual(userMessage4.Id,singleMessage.Id);
            Assert.AreEqual(userMessage4.TextMessage, singleMessage.TextMessage);
            Assert.AreEqual(userMessage4.FromAccount.Id, singleMessage.FromAccount.Id);
            Assert.AreEqual(userMessage4.FromAccount.Name, singleMessage.FromAccount.Name);
            Assert.AreEqual(userMessage4.ToAccount.Id, singleMessage.ToAccount.Id);           
        }

    }
}