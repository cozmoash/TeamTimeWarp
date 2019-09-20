using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;

namespace TeamTimeWarp.Rest.UnitTests.Controllers.Accounts
{
    [TestFixture]
    public class WhenAQuickLogonAccountIsCreated : AccountTestBase
    {
        private readonly HttpRequestMessage _request;
        private readonly AccountCreationResponse _result;

        public WhenAQuickLogonAccountIsCreated()
        {
            _request = HttpRequestMock.MockRequest();
            _result = AccountController.Post(_request, "quickLoginTest");
        }

        [Test]
        public void ThenTheNewAccountShouldBePersisted()
        {
            var stored = AccountRepository.GetAll().Single();

            Assert.AreEqual("quickLoginTest", stored.Name);
            Assert.AreEqual(string.Empty, stored.Email);
            Assert.AreEqual(AccountType.Quick, stored.AccountType);
        }

        [Test]
        public void ThenALoginTokeShouldBeReturned()
        {
            Assert.IsNotNull(_result.Token);
        }
    }
}