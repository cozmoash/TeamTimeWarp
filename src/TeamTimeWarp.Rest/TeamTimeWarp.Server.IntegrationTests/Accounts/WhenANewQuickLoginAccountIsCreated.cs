using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenANewQuickLoginAccountIsCreated : IntegrationTestBase
    {
        private readonly AccountCreationResponse _accountCreation;

        public WhenANewQuickLoginAccountIsCreated()
        {
            _accountCreation = CreateQuickLoginAccount();
        }

        [Test]
        public void ThenTheAccountIdIsReturned()
        {
            Assert.Greater(_accountCreation.AccountId, 0);
        }

        [Test]
        public void ThenALoginTokenIsReturned()
        {
            Assert.IsNotNullOrEmpty(_accountCreation.Token);
        }
    }
}