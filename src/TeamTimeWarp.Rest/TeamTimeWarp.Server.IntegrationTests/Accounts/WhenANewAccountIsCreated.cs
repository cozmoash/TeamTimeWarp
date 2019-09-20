using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{


    [TestFixture]
    public class WhenANewAccountIsCreated : IntegrationTestBase
    {
        private readonly AccountCreationResponse _accountCreation;

        public WhenANewAccountIsCreated()
        {
            _accountCreation = CreateAccount();
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